using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Interfaces;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public partial class ParkDetailViewModel : ObservableObject
{
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
    /// The entity IDs currently checked in the map filter tree. Defaults to empty
    /// (nothing shown) until the filter panel computes its first value, which
    /// happens synchronously on construction and includes every already-loaded
    /// child, so in practice pins never visibly "flash" all-then-filtered.
    /// </summary>
    [ObservableProperty]
    private IReadOnlySet<string> _visibleEntityIds = new HashSet<string>();

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



