using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Data.Entities;

namespace MagicWise.Desktop.ViewModels;

public partial class TagViewModel : ObservableObject
{
    private readonly Tag _tag;
    private readonly Action _onSelectionChanged;

    public TagViewModel(Tag tag, Action onSelectionChanged)
    {
        _tag = tag;
        _onSelectionChanged = onSelectionChanged;
    }

    public int Id { get { return _tag.Id; } }

    public string Name { get { return _tag.Name; } }

    [ObservableProperty]
    private bool _isSelected;

    partial void OnIsSelectedChanged(bool value)
    {
        _onSelectionChanged();
    }
}
