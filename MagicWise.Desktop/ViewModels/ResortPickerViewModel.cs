using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Interfaces;
using MagicWise.Core.Models;
using MagicWise.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// The "Pick a Resort" main-content page: search box, tag facet filters, and the
/// resort (destination) list. Notifies the owner (MainViewModel) when a resort
/// is chosen; picking a specific park within that resort happens afterwards, in
/// the hamburger menu's park picker.
/// </summary>
public partial class ResortPickerViewModel : ObservableObject
{
    private readonly IThemeParksAPI _api;
    private readonly MagicWiseDbContext _db;
    private readonly Action<DestinationViewModel> _onDestinationSelected;

    public ResortPickerViewModel(IThemeParksAPI api, MagicWiseDbContext db, Action<DestinationViewModel> onDestinationSelected)
    {
        _api = api;
        _db = db;
        _onDestinationSelected = onDestinationSelected;

        _destinations = new ObservableCollection<DestinationViewModel>();
        FilteredDestinations = CollectionViewSource.GetDefaultView(_destinations);
        FilteredDestinations.Filter = ApplyFilter;

        FilterPanel = new FilterPanelViewModel(Enumerable.Empty<TagCategoryViewModel>());

        _ = InitializeAsync();
    }

    // ── Collections ──────────────────────────────────────────────────────────

    private readonly ObservableCollection<DestinationViewModel> _destinations;

    public ICollectionView FilteredDestinations { get; }

    // ── Filter / Search ───────────────────────────────────────────────────────

    [ObservableProperty]
    private FilterPanelViewModel _filterPanel;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FilteredDestinations))]
    private string _searchText = string.Empty;

    /// <summary>
    /// True once the user has typed a search term or checked a tag filter.
    /// Drives the "search or filter to see resorts" empty-state hint.
    /// </summary>
    [ObservableProperty]
    private bool _hasSearchOrFilterCriteria;

    partial void OnSearchTextChanged(string value)
    {
        RefreshFilteredDestinations();
    }

    private void RefreshFilteredDestinations()
    {
        HasSearchOrFilterCriteria = !string.IsNullOrWhiteSpace(SearchText) || FilterPanel.HasActiveFilters;
        FilteredDestinations.Refresh();
    }

    private bool ApplyFilter(object obj)
    {
        if (obj is not DestinationViewModel destination)
        {
            return false;
        }

        bool hasSearch = !string.IsNullOrWhiteSpace(SearchText);
        bool hasActiveFilters = FilterPanel.HasActiveFilters;

        // Nothing is shown until the user starts searching or applies a tag
        // filter; an unfiltered list of every resort isn't useful up front.
        if (!hasSearch && !hasActiveFilters)
        {
            return false;
        }

        bool passesTagFilter = FilterPanel.Matches(destination.TagIds);
        if (!passesTagFilter)
        {
            return false;
        }

        if (!hasSearch)
        {
            return true;
        }

        string search = SearchText.Trim();
        return destination.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            || destination.Parks.Any(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    // ── Resort (destination) selection ──────────────────────────────────────

    public void SelectDestination(DestinationViewModel destination)
    {
        _onDestinationSelected(destination);
    }

    // ── Initialisation ────────────────────────────────────────────────────────

    private async Task InitializeAsync()
    {
        await LoadTagsAsync().ConfigureAwait(false);
        await LoadDestinationsAsync().ConfigureAwait(false);
    }

    private async Task LoadTagsAsync()
    {
        var categories = await _db.TagCategories
            .Include(c => c.Tags)
            .OrderBy(c => c.DisplayOrder)
            .ToListAsync()
            .ConfigureAwait(false);

        Action onFilterChanged = () =>
        {
            App.Current.Dispatcher.Invoke(RefreshFilteredDestinations);
        };

        var categoryVms = categories.Select(c => new TagCategoryViewModel(c, onFilterChanged));

        App.Current.Dispatcher.Invoke(() =>
        {
            FilterPanel = new FilterPanelViewModel(categoryVms);
        });
    }

    private async Task LoadDestinationsAsync()
    {
        var apiDestinations = await _api.GetDestinationsAsync().ConfigureAwait(false);
        if (apiDestinations == null)
        {
            return;
        }

        var allDestinationTags = await _db.DestinationTags
            .ToListAsync()
            .ConfigureAwait(false);

        var tagLookup = allDestinationTags
            .GroupBy(dt => dt.DestinationId, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.Select(dt => dt.TagId).ToList(), StringComparer.OrdinalIgnoreCase);

        App.Current.Dispatcher.Invoke(() =>
        {
            _destinations.Clear();
            foreach (var dest in apiDestinations.OrderBy(d => d.Name))
            {
                // DestinationTags are seeded keyed by the themeparks.wiki slug
                // (e.g. "waltdisneyworld"), not the GUID returned as Destination.Id,
                // so look up tags by slug first and fall back to Id for safety.
                List<int>? tagIds = null;
                if (!string.IsNullOrEmpty(dest.Slug))
                {
                    tagLookup.TryGetValue(dest.Slug, out tagIds);
                }

                if (tagIds == null)
                {
                    tagLookup.TryGetValue(dest.Id, out tagIds);
                }

                _destinations.Add(new DestinationViewModel(
                    dest.Id,
                    dest.Name,
                    dest.Parks ?? Enumerable.Empty<Park>(),
                    tagIds ?? Enumerable.Empty<int>()
                ));
            }
        });
    }
}
