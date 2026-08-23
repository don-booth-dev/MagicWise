using System.Collections.Generic;
using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

namespace MagicWise.Integrations.V1.Models;

public class EntityDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("entityType")]
    public EntityType EntityType { get; set; }

    [JsonPropertyName("parentId")]
    public string? ParentId { get; set; }

    [JsonPropertyName("destinationId")]
    public string? DestinationId { get; set; }

    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = null!;

    [JsonPropertyName("location")]
    public EntityLocationDto? Location { get; set; }

    [JsonPropertyName("slug")]
    public string? Slug { get; set; }

    [JsonPropertyName("externalId")]
    public string? ExternalId { get; set; }

    [JsonPropertyName("parkId")]
    public string? ParkId { get; set; }

    [JsonPropertyName("attractionType")]
    public AttractionType? AttractionType { get; set; }

    [JsonPropertyName("minimumHeight")]
    public int? MinimumHeight { get; set; }

    [JsonPropertyName("mayGetWet")]
    public bool? MayGetWet { get; set; }

    [JsonPropertyName("tags")]
    public List<TagDataDto>? Tags { get; set; }
}

