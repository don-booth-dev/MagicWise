using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class DestinationsResponseDto
{
    [JsonPropertyName("destinations")]
    public List<DestinationDto>? Destinations { get; set; }
}