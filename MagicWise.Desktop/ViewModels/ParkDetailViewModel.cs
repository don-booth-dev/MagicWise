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

    /// <summary>
    /// Bumped whenever wait times finish (re)loading, purely so the view can
    /// tell it needs to rebuild pin tooltips. The wait times themselves live on
    /// each <see cref="EntityChildViewModel"/>, not here.
    /// </summary>
    [ObservableProperty]
    private DateTime? _waitTimesUpdatedAt;

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

        await LoadWaitTimesAsync().ConfigureAwait(false);
    }

    private async Task LoadWaitTimesAsync()
    {
        try
        {
            var liveData = await _api.GetEntityLiveDataAsync(Park.Id).ConfigureAwait(false);
            if (liveData == null)
            {
                return;
            }

            var waitTimesById = liveData.ToDictionary(d => d.Id, d => d.StandbyWaitMinutes);

            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var child in Children)
                {
                    if (waitTimesById.TryGetValue(child.Id, out var waitMinutes))
                    {
                        child.WaitTimeMinutes = waitMinutes;
                    }
                }

                WaitTimesUpdatedAt = DateTime.UtcNow;
            });
        }
        catch
        {
            // Wait times are a nice-to-have overlay; failures here shouldn't
            // block the rest of the park view from working.
        }
    }
}



