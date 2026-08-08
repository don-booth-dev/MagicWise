using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class EntityScheduleEntryDto
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = null!;

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("openingTime")]
    public DateTime OpeningTime { get; set; }

    [JsonPropertyName("closingTime")]
    public DateTime ClosingTime { get; set; }

    [JsonExtensionData]
    public Dictionary<string, JsonElement>? AdditionalProperties { get; set; }
}