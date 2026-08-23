using MagicWise.Core.Models.Enums;

namespace MagicWise.Core.Models;

public class LiveData
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public EntityType EntityType { get; set; }

    public LiveStatus? Status { get; set; }

    public DateTime LastUpdated { get; set; }

    public int? StandbyWaitMinutes { get; set; }
}
