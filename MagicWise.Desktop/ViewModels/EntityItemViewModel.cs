using CommunityToolkit.Mvvm.ComponentModel;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// A single leaf checkbox in the map filter tree representing one specific
/// entity (e.g. a single ride or restaurant), nested under its
/// <see cref="EntityCategoryViewModel"/>.
/// </summary>
public partial class EntityItemViewModel : ObservableObject
{
    private readonly Action _onSelectionChanged;

    public EntityItemViewModel(string id, string name, bool isChecked, Action onSelectionChanged)
    {
        Id = id;
        Name = name;
        _isChecked = isChecked;
        _onSelectionChanged = onSelectionChanged;
    }

    public string Id { get; }

    public string Name { get; }

    [ObservableProperty]
    private bool _isChecked;

    partial void OnIsCheckedChanged(bool value)
    {
        _onSelectionChanged();
    }
}
