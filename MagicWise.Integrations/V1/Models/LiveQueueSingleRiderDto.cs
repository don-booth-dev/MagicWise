using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class LiveQueueSingleRiderDto
{
    [JsonPropertyName("waitTime")]
    public int? WaitTime { get; set; }
}


