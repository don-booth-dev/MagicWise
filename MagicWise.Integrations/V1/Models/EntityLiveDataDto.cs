using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

namespace MagicWise.Integrations.V1.Models;

public class EntityLiveDataDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("entityType")]
    public EntityType EntityType { get; set; }

    [JsonPropertyName("status")]
    public LiveStatusType? Status { get; set; }

    [JsonPropertyName("lastUpdated")]
    public DateTime LastUpdated { get; set; }

    [JsonPropertyName("queue")]
    public LiveQueueDto? Queue { get; set; }

    [JsonPropertyName("showtimes")]
    public List<LiveShowTimeDto>? Showtimes { get; set; }

    [JsonPropertyName("operatingHours")]
    public List<LiveShowTimeDto>? OperatingHours { get; set; }

    [JsonPropertyName("diningAvailability")]
    public List<object>? DiningAvailability { get; set; } // flexible; map to typed DTO if desired
}