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

        UpdatePins();
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

        UpdatePins();
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ParkDetailViewModel.Latitude) or nameof(ParkDetailViewModel.Longitude))
        {
            Dispatcher.Invoke(UpdatePins);
        }
    }

    private void OnChildrenChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        Dispatcher.Invoke(UpdatePins);
    }

    private void UpdatePins()
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
                Styles = { CreatePinStyle(Color.FromArgb(255, 30, 100, 220), 18) }
            });

            foreach (var child in _viewModel.Children)
            {
                if (child.Latitude.HasValue && child.Longitude.HasValue)
                {
                    var childPoint = SphericalMercator.FromLonLat(child.Longitude.Value, child.Latitude.Value).ToMPoint();
                    features.Add(new PointFeature(childPoint)
                    {
                        Styles = { CreatePinStyle(Color.FromArgb(200, 200, 80, 30), 12) }
                    });
                }
            }

            _pinLayer.Features = features;
            _pinLayer.DataHasChanged();

            MapControl.Map.Navigator.CenterOnAndZoomTo(parkPoint, MapControl.Map.Navigator.Resolutions[12]);
        }
        else
        {
            _pinLayer.Features = features;
            _pinLayer.DataHasChanged();
        }
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
