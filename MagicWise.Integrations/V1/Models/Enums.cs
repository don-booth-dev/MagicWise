using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1.Models.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EntityType
{
    DESTINATION,
    PARK,
    ATTRACTION,
    RESTAURANT,
    HOTEL,
    SHOW
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LiveStatusType
{
    OPERATING,
    DOWN,
    CLOSED,
    REFURBISHMENT
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LiveQueueType
{
    STANDBY,
    SINGLE_RIDER,
    RETURN_TIME,
    PAID_RETURN_TIME,
    BOARDING_GROUP,
    PAID_STANDBY
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BoardingGroupState
{
    AVAILABLE,
    PAUSED,
    CLOSED
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReturnTimeState
{
    AVAILABLE,
    TEMP_FULL,
    FINISHED
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SchedulePriceType
{
    ADMISSION,
    PACKAGE,
    ATTRACTION
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AttractionType
{
    RIDE,
    TRANSPORT,
    SHOW,
    EXPERIENCE
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PricedScheduleEntryType
{
    OPERATING,
    TICKETED_EVENT,
    PRIVATE_EVENT,
    EXTRA_HOURS,
    INFO
}

