using MagicWise.Core.Models.Enums;

namespace MagicWise.Core.Models;

public class Entity
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public EntityType EntityType { get; set; }

    public string? ParentId { get; set; }

    public string? DestinationId { get; set; }

    public string? ParkId { get; set; }

    public string Timezone { get; set; } = null!;

    public Location? Location { get; set; }

    public AttractionType? AttractionType { get; set; }

    public int? MinimumHeight { get; set; }

    public bool? MayGetWet { get; set; }
}
