namespace MagicWise.Core.Models;

public class Destination
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Slug { get; set; }

    public List<Park> Parks { get; set; } = new();
}
