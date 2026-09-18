using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// The hamburger (left) panel content shown once a resort (destination) has
/// been picked: a park picker at the top (single-select checkboxes, enforced
/// radio-button style) and — once a park is actually chosen — the existing
/// entity category filter tree nested beneath it.
/// </summary>
public partial class ResortFilterPanelViewModel : ObservableObject
{
    private readonly Action<ParkViewModel?> _onParkSelectionChanged;

    /// <summary>
    /// Guards against re-entrancy while this class is itself the one clearing
    /// out the other park checkboxes, so those cleared checkboxes don't also
    /// try to report a (spurious) deselection back up to the owner.
    /// </summary>
    private bool _isSyncingSelection;

    public ResortFilterPanelViewModel(DestinationViewModel destination, Action<ParkViewModel?> onParkSelectionChanged)
    {
        Destination = destination;
        _onParkSelectionChanged = onParkSelectionChanged;
        Parks = new ObservableCollection<ParkSelectionItemViewModel>(
            destination.Parks.Select(p => new ParkSelectionItemViewModel(p, OnParkCheckedChanged)));
    }

    public DestinationViewModel Destination { get; }

    public ObservableCollection<ParkSelectionItemViewModel> Parks { get; }

    /// <summary>
    /// The entity filter tree for the currently-selected park, or null while no
    /// park is selected — the map area then shows the default empty state.
    /// </summary>
    [ObservableProperty]
    private MapFilterPanelViewModel? _activeMapFilterPanel;

    private void OnParkCheckedChanged(ParkSelectionItemViewModel changed)
    {
        if (_isSyncingSelection)
        {
            return;
        }

        _isSyncingSelection = true;
        try
        {
            if (changed.IsChecked)
            {
                foreach (var other in Parks)
                {
                    if (other != changed)
                    {
                        other.IsChecked = false;
                    }
                }

                _onParkSelectionChanged(changed.Park);
            }
            else
            {
                _onParkSelectionChanged(null);
            }
        }
        finally
        {
            _isSyncingSelection = false;
        }
    }
}
