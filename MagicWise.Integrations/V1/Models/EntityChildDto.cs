using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

namespace MagicWise.Integrations.V1.Models;

public class EntityChildDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("entityType")]
    public EntityType EntityType { get; set; }

    [JsonPropertyName("externalId")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }

    [JsonPropertyName("location")]
    public EntityLocationDto? Location { get; set; }
}