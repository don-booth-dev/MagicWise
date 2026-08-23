using System.Collections.Generic;
using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

namespace MagicWise.Integrations.V1.Models;

public class ParkScheduleDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("entityType")]
    public EntityType? EntityType { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("schedule")]
    public List<PricedScheduleEntryDto>? Schedule { get; set; }
}

