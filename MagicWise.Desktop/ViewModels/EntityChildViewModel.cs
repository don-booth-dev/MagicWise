using MagicWise.Core.Models;

namespace MagicWise.Desktop.ViewModels;

public class EntityChildViewModel
{
    public EntityChildViewModel(EntityChild child)
    {
        Name = child.Name;
        Latitude = child.Location?.Latitude;
        Longitude = child.Location?.Longitude;
    }

    public string Name { get; }

    public double? Latitude { get; }

    public double? Longitude { get; }
}


