using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Interfaces;
using MagicWise.Core.Models.Enums;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public partial class ParkDetailViewModel : ObservableObject
{
    private static readonly IReadOnlySet<EntityType> AllEntityTypes = new HashSet<EntityType>
    {
        EntityType.Attraction,
        EntityType.Restaurant,
        EntityType.Hotel,
        EntityType.Show
    };

    private readonly IThemeParksAPI _api;

    public ParkDetailViewModel(IThemeParksAPI api, ParkViewModel park)
    {
        _api = api;
        Park = park;
        Children = new ObservableCollection<EntityChildViewModel>();
        _ = LoadAsync();
    }

    public ParkViewModel Park { get; }

    [ObservableProperty]
    private double? _latitude;

    [ObservableProperty]
    private double? _longitude;

    [ObservableProperty]
    private bool _isLoading;

    /// <summary>
    /// The entity types currently toggled on in the map filter panel. Defaults to
    /// all types so pins render before the filter panel makes its first update.
    /// </summary>
    [ObservableProperty]
    private IReadOnlySet<EntityType> _visibleEntityTypes = AllEntityTypes;

    public ObservableCollection<EntityChildViewModel> Children { get; }

    private async Task LoadAsync()
    {
        IsLoading = true;
        try
        {
            var entity = await _api.GetEntityAsync(Park.Id).ConfigureAwait(false);
            Latitude = entity?.Location?.Latitude;
            Longitude = entity?.Location?.Longitude;

            var children = await _api.GetEntityChildrenAsync(Park.Id).ConfigureAwait(false);
            if (children != null)
            {
                foreach (var child in children.Where(c => c.Location?.Latitude != null))
                {
                    App.Current.Dispatcher.Invoke(() => Children.Add(new EntityChildViewModel(child)));
                }
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}



