using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicWise.Data;
using MagicWise.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public partial class SettingsTagCategoryViewModel : ObservableObject
{
    private readonly MagicWiseDbContext _db;
    private readonly TagCategory _category;

    public SettingsTagCategoryViewModel(TagCategory category, MagicWiseDbContext db, IEnumerable<DestinationTag> allDestinationTags)
    {
        _category = category;
        _db = db;

        Tags = new ObservableCollection<SettingsTagViewModel>(
            category.Tags.OrderBy(t => t.Name)
                         .Select(t => new SettingsTagViewModel(t, db, this))
        );

        DestinationAssignments = new ObservableCollection<DestinationTagAssignmentViewModel>(
            allDestinationTags
                .Where(dt => category.Tags.Any(t => t.Id == dt.TagId))
                .GroupBy(dt => dt.DestinationId)
                .Select(g => new DestinationTagAssignmentViewModel(g.Key, category.Tags, g.Select(dt => dt.TagId).ToHashSet(), db))
        );
    }

    public int Id { get { return _category.Id; } }

    public string Name { get { return _category.Name; } }

    public ObservableCollection<SettingsTagViewModel> Tags { get; }

    public ObservableCollection<DestinationTagAssignmentViewModel> DestinationAssignments { get; }

    [ObservableProperty]
    private string _newTagName = string.Empty;

    [RelayCommand]
    private async Task AddTagAsync()
    {
        string name = NewTagName.Trim();
        if (string.IsNullOrEmpty(name))
        {
            return;
        }

        var tag = new Tag { Name = name, CategoryId = _category.Id };
        _db.Tags.Add(tag);
        await _db.SaveChangesAsync().ConfigureAwait(false);

        App.Current.Dispatcher.Invoke(() =>
        {
            Tags.Add(new SettingsTagViewModel(tag, _db, this));
            NewTagName = string.Empty;
        });
    }

    public void RemoveTag(SettingsTagViewModel tagVm)
    {
        Tags.Remove(tagVm);
    }
}
