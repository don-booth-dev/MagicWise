using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Data;
using MagicWise.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public partial class SettingsPageViewModel : ObservableObject
{
    private readonly MagicWiseDbContext _db;

    public SettingsPageViewModel(MagicWiseDbContext db)
    {
        _db = db;
        Categories = new ObservableCollection<SettingsTagCategoryViewModel>();
        _ = LoadAsync();
    }

    public ObservableCollection<SettingsTagCategoryViewModel> Categories { get; }

    [ObservableProperty]
    private bool _isLoading;

    private async Task LoadAsync()
    {
        IsLoading = true;
        try
        {
            var categories = await _db.TagCategories
                .Include(c => c.Tags)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync()
                .ConfigureAwait(false);

            var destinationTags = await _db.DestinationTags
                .ToListAsync()
                .ConfigureAwait(false);

            App.Current.Dispatcher.Invoke(() =>
            {
                Categories.Clear();
                foreach (var cat in categories)
                {
                    Categories.Add(new SettingsTagCategoryViewModel(cat, _db, destinationTags));
                }
            });
        }
        finally
        {
            IsLoading = false;
        }
    }

    public async Task ReloadAsync()
    {
        await LoadAsync().ConfigureAwait(false);
    }
}
