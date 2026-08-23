using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Data.Entities;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public partial class TagCategoryViewModel : ObservableObject
{
    private readonly TagCategory _category;
    private readonly Action _onFilterChanged;

    public TagCategoryViewModel(TagCategory category, Action onFilterChanged)
    {
        _category = category;
        _onFilterChanged = onFilterChanged;
        IsVisible = category.IsVisible;

        Tags = new ObservableCollection<TagViewModel>(
            category.Tags.OrderBy(t => t.Name)
                         .Select(t => new TagViewModel(t, onFilterChanged))
        );
    }

    public int Id { get { return _category.Id; } }

    public string Name { get { return _category.Name; } }

    [ObservableProperty]
    private bool _isVisible;

    public ObservableCollection<TagViewModel> Tags { get; }
}
