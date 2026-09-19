using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicWise.Core.Interfaces;
using MagicWise.Data;

namespace MagicWise.Desktop.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IThemeParksAPI _api;
    private readonly MagicWiseDbContext _db;

    private ParkDetailViewModel? _lastParkDetail;
    private MapFilterPanelViewModel? _lastMapFilterPanel;
    private ResortFilterPanelViewModel? _lastResortPanel;

    public MainViewModel(IThemeParksAPI api, MagicWiseDbContext db)
    {
        _api = api;
        _db = db;

        CurrentPage = CreateResortPicker();
    }

    // ── Page routing ──────────────────────────────────────────────────────────

    [ObservableProperty]
    private object _currentPage;

    /// <summary>
    /// Content shown above the fixed left-panel chrome (nav + attribution). Empty
    /// until a resort is selected, at which point it shows the resort's park
    /// picker (and, once a park is chosen there too, the map filter tree nested
    /// beneath it).
    /// </summary>
    [ObservableProperty]
    private object? _leftPanelContent;

    private ResortPickerViewModel CreateResortPicker()
    {
        return new ResortPickerViewModel(_api, _db, OnDestinationSelected);
    }

    private void OnDestinationSelected(DestinationViewModel destination)
    {
        var resortPanel = new ResortFilterPanelViewModel(destination, OnParkSelectionChanged);
        _lastResortPanel = resortPanel;

        LeftPanelContent = resortPanel;
        // No park chosen yet within the resort: show the default empty state.
        CurrentPage = new NoParkSelectedViewModel();
    }

    private void OnParkSelectionChanged(ParkViewModel? park)
    {
        if (park == null)
        {
            CurrentPage = new NoParkSelectedViewModel();
            if (_lastResortPanel != null)
            {
                _lastResortPanel.ActiveMapFilterPanel = null;
            }

            return;
        }

        var detail = new ParkDetailViewModel(_api, park);
        var filterPanel = new MapFilterPanelViewModel(park, detail.Children, ids => detail.VisibleEntityIds = ids);

        _lastParkDetail = detail;
        _lastMapFilterPanel = filterPanel;

        CurrentPage = detail;
        if (_lastResortPanel != null)
        {
            _lastResortPanel.ActiveMapFilterPanel = filterPanel;
        }
    }

    [RelayCommand]
    private void NavigateParks()
    {
        CurrentPage = CreateResortPicker();
        LeftPanelContent = null;
        _lastResortPanel = null;
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
}
