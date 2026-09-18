using MagicWise.Core.Models.Enums;
using MagicWise.Desktop.ViewModels;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Projections;
using Mapsui.Styles;
using Mapsui.Tiling;
using System.ComponentModel;
using System.Windows.Controls;

namespace MagicWise.Desktop.Views;

public partial class ParkDetailView : UserControl
{
    // Color + shape per entity type so categories are visually distinguishable
    // on the map without needing the filter panel open. Shapes are limited to
    // what Mapsui's SymbolStyle supports (Ellipse/Rectangle/Triangle) for now;
    // SymbolType.Image is available for custom per-type icon bitmaps later.
    private static readonly Dictionary<EntityType, (Color Color, SymbolType Shape)> EntityPinStyles = new()
    {
        [EntityType.Attraction] = (Color.FromArgb(255, 0, 120, 215), SymbolType.Ellipse),
        [EntityType.Restaurant] = (Color.FromArgb(255, 211, 84, 0), SymbolType.Rectangle),
        [EntityType.Show] = (Color.FromArgb(255, 155, 89, 182), SymbolType.Triangle),
        [EntityType.Hotel] = (Color.FromArgb(255, 26, 188, 156), SymbolType.Rectangle),
    };

    private static readonly Color ParkPinColor = Color.FromArgb(255, 34, 153, 84);

    private const int ParkPinSize = 14;
    private const int EntityPinSize = 9;

    private MemoryLayer? _pinLayer;
    private ParkDetailViewModel? _viewModel;

    /// <summary>
    /// The map resolution captured right after the view was last fit to the
    /// park (see <see cref="FitViewToPark"/>). Used as the 1.0x reference point
    /// for zoom-based pin scaling: zooming in from there enlarges pins, zooming
    /// out shrinks them.
    /// </summary>
    private double? _baselineResolution;

    private double _lastScaledResolution = -1;

    /// <summary>Current pin size multiplier driven by zoom level. 1.0 = baseline.</summary>
    private double _zoomSizeMultiplier = 1.0;

    public ParkDetailView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void MapControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
    {
        var map = new Map();
        map.Layers.Add(OpenStreetMap.CreateTileLayer());

        _pinLayer = new MemoryLayer { Name = "Pins", Style = null };
        map.Layers.Add(_pinLayer);

        map.Navigator.ViewportChanged += OnViewportChanged;

        MapControl.Map = map;

        UpdatePins(fitView: true);
    }

    private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            _viewModel.Children.CollectionChanged -= OnChildrenChanged;
        }

        _viewModel = DataContext as ParkDetailViewModel;

        if (_viewModel != null)
        {
            _viewModel.PropertyChanged += OnViewModelPropertyChanged;
            _viewModel.Children.CollectionChanged += OnChildrenChanged;
        }

        UpdatePins(fitView: true);
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ParkDetailViewModel.Latitude)
            or nameof(ParkDetailViewModel.Longitude))
        {
            // A new park's location just arrived: recenter on it (children aren't
            // loaded yet, so this is just a reasonable starting view).
            Dispatcher.Invoke(() => UpdatePins(fitView: true));
        }
        else if (e.PropertyName == nameof(ParkDetailViewModel.VisibleEntityIds))
        {
            // Filter toggles only change which pins are shown, never the camera.
            Dispatcher.Invoke(() => UpdatePins(fitView: false));
        }
        else if (e.PropertyName == nameof(ParkDetailViewModel.IsLoading)
            && _viewModel?.IsLoading == false)
        {
            // All children have finished loading: fit the view to show the whole park.
            Dispatcher.Invoke(() => UpdatePins(fitView: true));
        }
    }

    private void OnChildrenChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        // New pins arrived mid-load; just redraw them without disturbing the camera.
        // The final fit-to-park happens once loading completes (see IsLoading above).
        Dispatcher.Invoke(() => UpdatePins(fitView: false));
    }

    private void OnViewportChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_baselineResolution is not double baseline || MapControl.Map == null)
        {
            return;
        }

        var currentResolution = MapControl.Map.Navigator.Viewport.Resolution;
        if (currentResolution <= 0)
        {
            return;
        }

        // Panning fires this event too but leaves resolution unchanged; skip the
        // (relatively expensive) pin rebuild unless the zoom level actually moved.
        if (_lastScaledResolution > 0
            && Math.Abs(currentResolution - _lastScaledResolution) / currentResolution < 0.01)
        {
            return;
        }

        _lastScaledResolution = currentResolution;

        // Zooming in halves the resolution each level, so pins should grow at the
        // same rate; clamp so they never become illegibly tiny or absurdly huge.
        _zoomSizeMultiplier = Math.Clamp(baseline / currentResolution, 0.5, 2.5);

        Dispatcher.Invoke(() => UpdatePins(fitView: false));
    }

    private void UpdatePins(bool fitView)
    {
        if (_pinLayer == null || MapControl.Map == null || _viewModel == null)
        {
            return;
        }

        var features = new List<IFeature>();

        if (_viewModel.Latitude.HasValue && _viewModel.Longitude.HasValue)
        {
            var parkPoint = SphericalMercator.FromLonLat(_viewModel.Longitude.Value, _viewModel.Latitude.Value).ToMPoint();
            features.Add(new PointFeature(parkPoint)
            {
                Styles = { CreatePinStyle(ParkPinColor, SymbolType.Ellipse, ParkPinSize, _zoomSizeMultiplier) }
            });

            var allChildPoints = new List<MPoint>();
            foreach (var child in _viewModel.Children)
            {
                if (child.Latitude.HasValue && child.Longitude.HasValue)
                {
                    var childPoint = SphericalMercator.FromLonLat(child.Longitude.Value, child.Latitude.Value).ToMPoint();
                    allChildPoints.Add(childPoint);

                    if (_viewModel.VisibleEntityIds.Contains(child.Id))
                    {
                        var (color, shape) = EntityPinStyles.TryGetValue(child.EntityType, out var style)
                            ? style
                            : (Color.FromArgb(220, 120, 120, 120), SymbolType.Ellipse);

                        features.Add(new PointFeature(childPoint)
                        {
                            Styles = { CreatePinStyle(color, shape, EntityPinSize, _zoomSizeMultiplier) }
                        });
                    }
                }
            }

            _pinLayer.Features = features;
            _pinLayer.DataHasChanged();

            if (fitView)
            {
                FitViewToPark(parkPoint, allChildPoints);
            }
        }
        else
        {
            _pinLayer.Features = features;
            _pinLayer.DataHasChanged();
        }
    }

    private void FitViewToPark(MPoint parkPoint, IReadOnlyList<MPoint> allChildPoints)
    {
        // Duration 0 makes the fit apply instantly so Navigator.Viewport.Resolution
        // below reflects the new zoom level immediately, giving us a reliable
        // baseline for zoom-based pin scaling.
        if (allChildPoints.Count == 0)
        {
            // No children loaded (yet); just center on the park at a reasonable zoom level.
            MapControl.Map!.Navigator.CenterOnAndZoomTo(parkPoint, MapControl.Map.Navigator.Resolutions[12], 0);
        }
        else
        {
            var minX = parkPoint.X;
            var maxX = parkPoint.X;
            var minY = parkPoint.Y;
            var maxY = parkPoint.Y;

            foreach (var point in allChildPoints)
            {
                minX = Math.Min(minX, point.X);
                maxX = Math.Max(maxX, point.X);
                minY = Math.Min(minY, point.Y);
                maxY = Math.Max(maxY, point.Y);
            }

            // Pad the bounds a bit so edge pins aren't clipped against the viewport border.
            var paddingX = Math.Max((maxX - minX) * 0.15, 100);
            var paddingY = Math.Max((maxY - minY) * 0.15, 100);
            var box = new MRect(minX - paddingX, minY - paddingY, maxX + paddingX, maxY + paddingY);

            MapControl.Map!.Navigator.ZoomToBox(box, MBoxFit.Fit, 0);
        }

        _baselineResolution = MapControl.Map!.Navigator.Viewport.Resolution;
        _lastScaledResolution = _baselineResolution.Value;
        _zoomSizeMultiplier = 1.0;
    }

    private static SymbolStyle CreatePinStyle(Color color, SymbolType shape, int size, double scaleMultiplier)
    {
        return new SymbolStyle
        {
            SymbolType = shape,
            Fill = new Brush(color),
            Outline = new Pen(Color.White, 2),
            SymbolScale = (size / 20.0) * scaleMultiplier
        };
    }
}
