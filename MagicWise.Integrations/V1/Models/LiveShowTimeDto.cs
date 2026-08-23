using System;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class LiveShowTimeDto
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = null!;

    [JsonPropertyName("startTime")]
    public DateTime? StartTime { get; set; }

    [JsonPropertyName("endTime")]
    public DateTime? EndTime { get; set; }
}

