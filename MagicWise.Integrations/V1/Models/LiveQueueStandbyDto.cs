using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class LiveQueueStandbyDto
{
    [JsonPropertyName("waitTime")]
    public int? WaitTime { get; set; }
}

