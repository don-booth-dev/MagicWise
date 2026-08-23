namespace MagicWise.Core.Models;

public class ScheduleEntry
{
    public string Date { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime OpeningTime { get; set; }

    public DateTime ClosingTime { get; set; }
}
