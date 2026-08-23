using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

namespace MagicWise.Integrations.V1.Models;

public class SchedulePriceObjectDto
{
    [JsonPropertyName("type")]
    public SchedulePriceType? Type { get; set; }

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("price")]
    public PriceDataDto? Price { get; set; }

    [JsonPropertyName("available")]
    public bool? Available { get; set; }
}

