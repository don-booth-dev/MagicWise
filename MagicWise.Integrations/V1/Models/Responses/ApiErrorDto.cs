using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models.Responses;

public class ApiErrorDto
{
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }
}