using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicWise.Data;
using MagicWise.Data.Entities;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

/// <summary>
/// Represents a single destination row in the Settings tag-assignment section,
/// showing which tags from one category are assigned to that destination.
/// </summary>
public partial class DestinationTagAssignmentViewModel : ObservableObject
{
    private readonly MagicWiseDbContext _db;
    private readonly IEnumerable<Tag> _categoryTags;
    private readonly HashSet<int> _assignedTagIds;

    public DestinationTagAssignmentViewModel(string destinationId, IEnumerable<Tag> categoryTags, HashSet<int> assignedTagIds, MagicWiseDbContext db)
    {
        DestinationId = destinationId;
        _db = db;
        _categoryTags = categoryTags;
        _assignedTagIds = assignedTagIds;

        AssignedTags = new ObservableCollection<TagAssignmentItemViewModel>(
            categoryTags.Select(t => new TagAssignmentItemViewModel(t, assignedTagIds.Contains(t.Id), destinationId, db))
        );
    }

    public string DestinationId { get; }

    public ObservableCollection<TagAssignmentItemViewModel> AssignedTags { get; }
}
