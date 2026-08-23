using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class DiningAvailabilityDto
{
    [JsonPropertyName("partySize")]
    public decimal? PartySize { get; set; }

    [JsonPropertyName("waitTime")]
    public decimal? WaitTime { get; set; }
}


