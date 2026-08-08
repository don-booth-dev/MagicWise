using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class EntityLocationDto
{
    [JsonPropertyName("latitude")]
    public double? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; set; }
}