using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class DestinationDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("externalId")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("parks")]
    public List<ParkDto>? Parks { get; set; }
}