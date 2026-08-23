using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public partial class FilterPanelViewModel : ObservableObject
{
    public FilterPanelViewModel(IEnumerable<TagCategoryViewModel> categories)
    {
        Categories = new ObservableCollection<TagCategoryViewModel>(categories);
    }

    public ObservableCollection<TagCategoryViewModel> Categories { get; }

    public bool HasActiveFilters
    {
        get
        {
            return Categories.Any(c => c.Tags.Any(t => t.IsSelected));
        }
    }

    /// <summary>
    /// Returns true if the given destination (by id) passes all active filter facets.
    /// OR logic within a category; AND logic across categories.
    /// A category with no selections is ignored (all pass).
    /// </summary>
    public bool Matches(IEnumerable<int> destinationTagIds)
    {
        HashSet<int> tagIdSet = new HashSet<int>(destinationTagIds);

        foreach (TagCategoryViewModel category in Categories)
        {
            List<TagViewModel> selectedTags = category.Tags.Where(t => t.IsSelected).ToList();
            if (selectedTags.Count == 0)
            {
                continue;
            }

            bool anyMatch = selectedTags.Any(t => tagIdSet.Contains(t.Id));
            if (!anyMatch)
            {
                return false;
            }
        }

        return true;
    }
}
