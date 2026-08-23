namespace MagicWise.Core.Models;

public class Schedule
{
    public string? Id { get; set; }

    public string? Name { get; set; }

    public string? Timezone { get; set; }

    public List<ScheduleEntry> Entries { get; set; } = new();
}
