using System;
using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

namespace MagicWise.Integrations.V1.Models;

public class LiveQueueReturnTimeDto
{
    [JsonPropertyName("state")]
    public ReturnTimeState State { get; set; }

    [JsonPropertyName("returnStart")]
    public DateTime? ReturnStart { get; set; }

    [JsonPropertyName("returnEnd")]
    public DateTime? ReturnEnd { get; set; }
}