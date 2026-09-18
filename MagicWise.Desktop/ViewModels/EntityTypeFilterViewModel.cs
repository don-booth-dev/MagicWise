using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Models.Enums;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// A single checkbox-bound entity type toggle shown in the map filter panel
/// (e.g. Attractions, Restaurants, Hotels, Shows).
/// </summary>
public partial class EntityTypeFilterViewModel : ObservableObject
{
    private readonly Action _onSelectionChanged;

    public EntityTypeFilterViewModel(EntityType entityType, string displayName, Action onSelectionChanged, bool isChecked = true)
    {
        EntityType = entityType;
        DisplayName = displayName;
        _onSelectionChanged = onSelectionChanged;
        _isChecked = isChecked;
    }

    public EntityType EntityType { get; }

    public string DisplayName { get; }

    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value)
    {
        _onSelectionChanged();
    }
}
