using MagicWise.Core.Models.Enums;

namespace MagicWise.Core.Models;

public class EntityChild
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public EntityType EntityType { get; set; }

    public string? ParentId { get; set; }

    public Location? Location { get; set; }
}
