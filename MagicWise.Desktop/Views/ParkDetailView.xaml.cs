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
    private MemoryLayer? _pinLayer;
    private ParkDetailViewModel? _viewModel;

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
        else if (e.PropertyName == nameof(ParkDetailViewModel.VisibleEntityTypes))
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
                Styles = { CreatePinStyle(Color.FromArgb(255, 30, 100, 220), 9) }
            });

            var allChildPoints = new List<MPoint>();
            foreach (var child in _viewModel.Children)
            {
                if (child.Latitude.HasValue && child.Longitude.HasValue)
                {
                    var childPoint = SphericalMercator.FromLonLat(child.Longitude.Value, child.Latitude.Value).ToMPoint();
                    allChildPoints.Add(childPoint);

                    if (_viewModel.VisibleEntityTypes.Contains(child.EntityType))
                    {
                        features.Add(new PointFeature(childPoint)
                        {
                            Styles = { CreatePinStyle(Color.FromArgb(200, 200, 80, 30), 6) }
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
        if (allChildPoints.Count == 0)
        {
            // No children loaded (yet); just center on the park at a reasonable zoom level.
            MapControl.Map!.Navigator.CenterOnAndZoomTo(parkPoint, MapControl.Map.Navigator.Resolutions[12]);
            return;
        }

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

        MapControl.Map!.Navigator.ZoomToBox(box, MBoxFit.Fit);
    }

    private static SymbolStyle CreatePinStyle(Color color, int size)
    {
        return new SymbolStyle
        {
            SymbolType = SymbolType.Ellipse,
            Fill = new Brush(color),
            Outline = new Pen(Color.White, 2),
            SymbolScale = size / 20.0
        };
    }
}
