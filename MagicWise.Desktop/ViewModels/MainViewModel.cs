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

    public MainViewModel(IThemeParksAPI api, MagicWiseDbContext db)
    {
        _api = api;
        _db = db;

        CurrentPage = CreateParkPicker();
    }

    // ── Page routing ──────────────────────────────────────────────────────────

    [ObservableProperty]
    private object _currentPage;

    /// <summary>
    /// Content shown above the fixed left-panel chrome (nav + attribution). Empty
    /// until a park is selected, at which point it shows the map filter panel.
    /// </summary>
    [ObservableProperty]
    private object? _leftPanelContent;

    private ParkPickerViewModel CreateParkPicker()
    {
        return new ParkPickerViewModel(_api, _db, OnParkSelected);
    }

    private void OnParkSelected(ParkViewModel park)
    {
        var detail = new ParkDetailViewModel(_api, park);
        var filterPanel = new MapFilterPanelViewModel(park, detail.Children, ids => detail.VisibleEntityIds = ids);

        _lastParkDetail = detail;
        _lastMapFilterPanel = filterPanel;

        CurrentPage = detail;
        LeftPanelContent = filterPanel;
    }

    [RelayCommand]
    private void NavigateParks()
    {
        CurrentPage = CreateParkPicker();
        LeftPanelContent = null;
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
