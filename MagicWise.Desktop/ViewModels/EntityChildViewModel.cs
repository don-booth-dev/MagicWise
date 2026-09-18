using MagicWise.Core.Models;
using MagicWise.Core.Models.Enums;

namespace MagicWise.Desktop.ViewModels;

public class EntityChildViewModel
{
    public EntityChildViewModel(EntityChild child)
    {
        Id = child.Id;
        Name = child.Name;
        EntityType = child.EntityType;
        Latitude = child.Location?.Latitude;
        Longitude = child.Location?.Longitude;
    }

    public string Id { get; }

    public string Name { get; }

    public EntityType EntityType { get; }

    public double? Latitude { get; }

    public double? Longitude { get; }
}


