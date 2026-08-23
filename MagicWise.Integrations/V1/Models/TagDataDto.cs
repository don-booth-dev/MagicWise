using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class TagDataDto
{
    [JsonPropertyName("tag")]
    public string Tag { get; set; } = null!;

    [JsonPropertyName("tagName")]
    public string TagName { get; set; } = null!;

    [JsonPropertyName("id")]
    public string? Id { get; set; }

    // value may be string, number or object - use JsonElement for flexibility
    [JsonPropertyName("value")]
    public JsonElement? Value { get; set; }
}

