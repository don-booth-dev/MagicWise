using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class PricedScheduleEntryDto
{
    [JsonPropertyName("date")]
    public string Date { get; set; } = null!; // YYYY-MM-DD

    [JsonPropertyName("openingTime")]
    public DateTime OpeningTime { get; set; }

    [JsonPropertyName("closingTime")]
    public DateTime ClosingTime { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("purchases")]
    public List<SchedulePriceObjectDto>? Purchases { get; set; }
}