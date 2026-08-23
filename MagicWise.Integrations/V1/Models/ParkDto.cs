using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class ParkDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}

