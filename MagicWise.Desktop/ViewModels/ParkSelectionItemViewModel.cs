using CommunityToolkit.Mvvm.ComponentModel;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// A single park entry in the resort-level park picker shown at the top of the
/// hamburger menu. Checkboxes here behave like radio buttons: checking one park
/// unchecks any other (enforced by the owning <see cref="ResortFilterPanelViewModel"/>),
/// so at most one park is ever selected at a time.
/// </summary>
public partial class ParkSelectionItemViewModel : ObservableObject
{
    private readonly Action<ParkSelectionItemViewModel> _onCheckedChanged;

    public ParkSelectionItemViewModel(ParkViewModel park, Action<ParkSelectionItemViewModel> onCheckedChanged)
    {
        Park = park;
        _onCheckedChanged = onCheckedChanged;
    }

    public ParkViewModel Park { get; }

    public string Name => Park.Name;

    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value)
    {
        _onCheckedChanged(this);
    }
}
