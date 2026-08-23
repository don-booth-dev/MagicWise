using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MagicWise.Data;
using MagicWise.Data.Entities;

namespace MagicWise.Desktop.ViewModels;

public partial class SettingsTagViewModel : ObservableObject
{
    private readonly Tag _tag;
    private readonly MagicWiseDbContext _db;
    private readonly SettingsTagCategoryViewModel _parent;

    public SettingsTagViewModel(Tag tag, MagicWiseDbContext db, SettingsTagCategoryViewModel parent)
    {
        _tag = tag;
        _db = db;
        _parent = parent;
        _name = tag.Name;
    }

    public int Id { get { return _tag.Id; } }

    [ObservableProperty]
    private string _name;

    partial void OnNameChanged(string value)
    {
        _tag.Name = value;
        _ = _db.SaveChangesAsync();
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        _db.Tags.Remove(_tag);
        await _db.SaveChangesAsync().ConfigureAwait(false);
        App.Current.Dispatcher.Invoke(() => _parent.RemoveTag(this));
    }
}
