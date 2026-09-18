using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Models.Enums;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// A category node in the map filter tree (e.g. "Attractions"). Expands to list
/// every individual entity of that type as a child checkbox. The category's own
/// checkbox is tri-state: checked/unchecked selects or clears every item beneath
/// it, and shows indeterminate when only some items are checked.
/// </summary>
public partial class EntityCategoryViewModel : ObservableObject
{
    private readonly Action _onSelectionChanged;

    /// <summary>
    /// Guards against re-entrancy: true while we're setting <see cref="IsChecked"/>
    /// ourselves (from <see cref="RecomputeIsCheckedFromChildren"/>), so that the
    /// resulting property-changed callback doesn't try to cascade back down to
    /// the children that just caused the recompute.
    /// </summary>
    private bool _isUpdatingFromChildren;

    public EntityCategoryViewModel(EntityType entityType, string displayName, Action onSelectionChanged)
    {
        EntityType = entityType;
        DisplayName = displayName;
        _onSelectionChanged = onSelectionChanged;
        Items = new ObservableCollection<EntityItemViewModel>();
        Items.CollectionChanged += OnItemsCollectionChanged;
    }

    public EntityType EntityType { get; }

    public string DisplayName { get; }

    public ObservableCollection<EntityItemViewModel> Items { get; }

    /// <summary>
    /// True if every item is checked, false if none are, null (indeterminate) if
    /// only some are. Setting this explicitly to true/false cascades to every item.
    /// </summary>
    [ObservableProperty]
    private bool? _isChecked = true;

    public void AddItem(string id, string name)
    {
        // New items inherit whatever the category's current effective state is:
        // checked unless the user has explicitly cleared the whole category.
        var item = new EntityItemViewModel(id, name, IsChecked != false, OnItemSelectionChanged);
        Items.Add(item);
    }

    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        RecomputeIsCheckedFromChildren();
    }

    private void OnItemSelectionChanged()
    {
        RecomputeIsCheckedFromChildren();
        _onSelectionChanged();
    }

    private void RecomputeIsCheckedFromChildren()
    {
        if (Items.Count == 0)
        {
            return;
        }

        var checkedCount = Items.Count(i => i.IsChecked);

        _isUpdatingFromChildren = true;
        IsChecked = checkedCount == 0 ? false : checkedCount == Items.Count ? true : null;
        _isUpdatingFromChildren = false;
    }

    partial void OnIsCheckedChanged(bool? value)
    {
        if (_isUpdatingFromChildren)
        {
            return;
        }

        // A tri-state checkbox briefly passes through indeterminate on its way
        // from checked to unchecked when clicked directly; treat that the same
        // as an explicit "select none" rather than leaving it ambiguous.
        var selectAll = value ?? false;

        foreach (var item in Items)
        {
            item.IsChecked = selectAll;
        }

        _onSelectionChanged();
    }
}
