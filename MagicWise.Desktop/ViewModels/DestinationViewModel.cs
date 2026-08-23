using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Models;
using System.Collections.ObjectModel;

namespace MagicWise.Desktop.ViewModels;

public class DestinationViewModel : ObservableObject
{
    public DestinationViewModel(string id, string name, IEnumerable<Park> parks, IEnumerable<int> tagIds)
    {
        Id = id;
        Name = name;
        TagIds = tagIds.ToList();
        Parks = new ObservableCollection<ParkViewModel>(
            parks.Select(p => new ParkViewModel(p))
        );
    }

    public string Id { get; }

    public string Name { get; }

    /// <summary>
    /// Tag ids assigned to this destination in the local database.
    /// All parks within this destination inherit these for filtering.
    /// </summary>
    public IReadOnlyList<int> TagIds { get; }

    public ObservableCollection<ParkViewModel> Parks { get; }
}


