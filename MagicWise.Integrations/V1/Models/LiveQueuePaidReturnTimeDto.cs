using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class LiveQueuePaidReturnTimeDto : LiveQueueReturnTimeDto
{
    [JsonPropertyName("price")]
    public PriceDataDto? Price { get; set; }
}

