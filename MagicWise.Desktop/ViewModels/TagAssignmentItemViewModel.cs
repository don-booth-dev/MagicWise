using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Data;
using MagicWise.Data.Entities;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public partial class TagAssignmentItemViewModel : ObservableObject
{
    private readonly Tag _tag;
    private readonly string _destinationId;
    private readonly MagicWiseDbContext _db;

    public TagAssignmentItemViewModel(Tag tag, bool isAssigned, string destinationId, MagicWiseDbContext db)
    {
        _tag = tag;
        _destinationId = destinationId;
        _db = db;
        _isAssigned = isAssigned;
    }

    public string TagName { get { return _tag.Name; } }

    [ObservableProperty]
    private bool _isAssigned;

    partial void OnIsAssignedChanged(bool value)
    {
        _ = SaveAsync(value);
    }

    private async Task SaveAsync(bool assigned)
    {
        if (assigned)
        {
            bool exists = _db.DestinationTags
                .Any(dt => dt.DestinationId == _destinationId && dt.TagId == _tag.Id);

            if (!exists)
            {
                _db.DestinationTags.Add(new DestinationTag
                {
                    DestinationId = _destinationId,
                    TagId = _tag.Id
                });
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
        else
        {
            var existing = _db.DestinationTags
                .FirstOrDefault(dt => dt.DestinationId == _destinationId && dt.TagId == _tag.Id);

            if (existing != null)
            {
                _db.DestinationTags.Remove(existing);
                await _db.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
