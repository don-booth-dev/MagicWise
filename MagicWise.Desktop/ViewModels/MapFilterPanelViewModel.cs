using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Models.Enums;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// The hamburger (left) panel content shown once a park has been selected: the
/// park's name plus checkboxes for which entity types render as pins on the map.
/// </summary>
public partial class MapFilterPanelViewModel : ObservableObject
{
    private readonly Action<IReadOnlySet<EntityType>> _onVisibleTypesChanged;

    public MapFilterPanelViewModel(ParkViewModel park, Action<IReadOnlySet<EntityType>> onVisibleTypesChanged)
    {
        Park = park;
        _onVisibleTypesChanged = onVisibleTypesChanged;

        Filters = new ObservableCollection<EntityTypeFilterViewModel>
        {
            new(EntityType.Attraction, "Attractions", RaiseVisibleTypesChanged),
            new(EntityType.Restaurant, "Restaurants", RaiseVisibleTypesChanged),
            new(EntityType.Hotel, "Hotels", RaiseVisibleTypesChanged),
            new(EntityType.Show, "Shows", RaiseVisibleTypesChanged)
        };

        RaiseVisibleTypesChanged();
    }

    public ParkViewModel Park { get; }

    public ObservableCollection<EntityTypeFilterViewModel> Filters { get; }

    private void RaiseVisibleTypesChanged()
    {
        var visible = Filters.Where(f => f.IsChecked).Select(f => f.EntityType).ToHashSet();
        _onVisibleTypesChanged(visible);
    }
}
