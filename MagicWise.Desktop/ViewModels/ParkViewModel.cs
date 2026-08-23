using CommunityToolkit.Mvvm.ComponentModel;
using MagicWise.Core.Models;

namespace MagicWise.Desktop.ViewModels;

public class ParkViewModel : ObservableObject
{
    private readonly Park _park;

    public ParkViewModel(Park park)
    {
        _park = park;
    }

    public string Id { get { return _park.Id; } }

    public string Name { get { return _park.Name; } }
}


