using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class DestinationsDto
{
    [JsonPropertyName("destinations")]
    public List<DestinationDto>? Destinations { get; set; }
}

