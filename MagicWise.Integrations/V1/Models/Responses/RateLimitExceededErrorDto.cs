using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models.Responses;

public class RateLimitExceededErrorDto : ApiErrorDto
{
    [JsonPropertyName("retryAfter")]
    public int? RetryAfter { get; set; }
}