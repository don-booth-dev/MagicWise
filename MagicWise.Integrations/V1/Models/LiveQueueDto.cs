using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models;

public class LiveQueueDto
{
    [JsonPropertyName("STANDBY")]
    public LiveQueueStandbyDto? Standby { get; set; }

    [JsonPropertyName("RETURN_TIME")]
    public LiveQueueReturnTimeDto? ReturnTime { get; set; }

    [JsonPropertyName("PAID_RETURN_TIME")]
    public LiveQueuePaidReturnTimeDto? PaidReturnTime { get; set; }

    [JsonPropertyName("BOARDING_GROUP")]
    public LiveQueueBoardingGroupDto? BoardingGroup { get; set; }

    [JsonPropertyName("PAID_STANDBY")]
    public LiveQueuePaidStandbyDto? PaidStandby { get; set; }
}