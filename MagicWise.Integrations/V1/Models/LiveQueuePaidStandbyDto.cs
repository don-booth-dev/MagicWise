using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class LiveQueuePaidStandbyDto
{
    [JsonPropertyName("waitTime")]
    public int? WaitTime { get; set; }
}