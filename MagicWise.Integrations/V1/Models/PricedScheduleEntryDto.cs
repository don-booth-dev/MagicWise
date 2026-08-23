using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

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
    public PricedScheduleEntryType Type { get; set; }

    [JsonPropertyName("purchases")]
    public List<SchedulePriceObjectDto>? Purchases { get; set; }
}

