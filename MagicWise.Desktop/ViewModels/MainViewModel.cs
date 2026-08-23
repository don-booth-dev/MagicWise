using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicWise.Core.Interfaces;
using MagicWise.Core.Models;
using MagicWise.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace MagicWise.Desktop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IThemeParksAPI _api;
    private readonly MagicWiseDbContext _db;

    private ParkDetailViewModel? _lastParkDetail;

    public MainViewModel(IThemeParksAPI api, MagicWiseDbContext db)
    {
        _api = api;
        _db = db;

        _destinations = new ObservableCollection<DestinationViewModel>();
        FilteredDestinations = CollectionViewSource.GetDefaultView(_destinations);
        FilteredDestinations.Filter = ApplyFilter;

        FilterPanel = new FilterPanelViewModel(Enumerable.Empty<TagCategoryViewModel>());
        CurrentPage = new WelcomePageViewModel();

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

    partial void OnSearchTextChanged(string value)
    {
        FilteredDestinations.Refresh();
    }

    private bool ApplyFilter(object obj)
    {
        if (obj is not DestinationViewModel destination)
        {
            return false;
        }

        bool passesTagFilter = FilterPanel.Matches(destination.TagIds);
        if (!passesTagFilter)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            return true;
        }

        string search = SearchText.Trim();
        return destination.Name.Contains(search, StringComparison.OrdinalIgnoreCase)
            || destination.Parks.Any(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    // ── Park selection ────────────────────────────────────────────────────────

    [ObservableProperty]
    private ParkViewModel? _selectedPark;

    partial void OnSelectedParkChanged(ParkViewModel? value)
    {
        if (value == null)
        {
            return;
        }

        var detail = new ParkDetailViewModel(_api, value);
        _lastParkDetail = detail;
        CurrentPage = detail;
    }

    // ── Page routing ──────────────────────────────────────────────────────────

    [ObservableProperty]
    private object _currentPage;

    [RelayCommand]
    private void NavigateParks()
    {
        CurrentPage = (object?)_lastParkDetail ?? new WelcomePageViewModel();
    }

    [RelayCommand]
    private void NavigateSettings()
    {
        CurrentPage = new SettingsPageViewModel(_db);
    }

    // ── Left panel toggle ─────────────────────────────────────────────────────

    [ObservableProperty]
    private bool _isPanelOpen = true;

    [RelayCommand]
    private void TogglePanel()
    {
        IsPanelOpen = !IsPanelOpen;
    }

    // ── Login stubs (Google OAuth — not yet implemented) ─────────────────────

    [ObservableProperty]
    private bool _isLoggedIn = false;

    [ObservableProperty]
    private string _userDisplayName = "Sign In";

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
            App.Current.Dispatcher.Invoke(() => FilteredDestinations.Refresh());
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
                tagLookup.TryGetValue(dest.Id, out var tagIds);
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



