using System;
using System.Text.Json.Serialization;
using MagicWise.Integrations.V1.Models.Enums;

namespace MagicWise.Integrations.V1.Models;

public class LiveQueueBoardingGroupDto
{
    [JsonPropertyName("allocationStatus")]
    public BoardingGroupState AllocationStatus { get; set; }

    [JsonPropertyName("currentGroupStart")]
    public int? CurrentGroupStart { get; set; }

    [JsonPropertyName("currentGroupEnd")]
    public int? CurrentGroupEnd { get; set; }

    [JsonPropertyName("nextAllocationTime")]
    public DateTime? NextAllocationTime { get; set; }

    [JsonPropertyName("estimatedWait")]
    public int? EstimatedWait { get; set; }
}