using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class PriceDataDto
{
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = null!;

    [JsonPropertyName("formatted")]
    public string? Formatted { get; set; }
}