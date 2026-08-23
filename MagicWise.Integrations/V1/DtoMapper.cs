using MagicWise.Core.Models;
using MagicWise.Integrations.V1.Models;
using MagicWise.Integrations.V1.Models.Enums;
using CoreEnums = MagicWise.Core.Models.Enums;

namespace MagicWise.Integrations.V1;

internal static class DtoMapper
{
    public static LiveData ToDomain(this EntityLiveDataDto dto)
    {
        return new LiveData
        {
            Id = dto.Id,
            Name = dto.Name,
            EntityType = dto.EntityType.ToDomain(),
            Status = dto.Status.HasValue ? dto.Status.Value.ToDomain() : null,
            LastUpdated = dto.LastUpdated,
            StandbyWaitMinutes = dto.Queue?.Standby?.WaitTime
        };
    }

    public static Destination ToDomain(this DestinationDto dto)
    {
        return new Destination
        {
            Id = dto.Id,
            Name = dto.Name,
            Slug = dto.Slug,
            Parks = dto.Parks?.Select(p => p.ToDomain()).ToList() ?? new List<Park>()
        };
    }

    public static Park ToDomain(this ParkDto dto)
    {
        return new Park
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }

    public static Entity ToDomain(this EntityDto dto)
    {
        return new Entity
        {
            Id = dto.Id,
            Name = dto.Name,
            EntityType = dto.EntityType.ToDomain(),
            ParentId = dto.ParentId,
            DestinationId = dto.DestinationId,
            ParkId = dto.ParkId,
            Timezone = dto.Timezone,
            Location = dto.Location?.ToDomain(),
            AttractionType = dto.AttractionType?.ToDomain(),
            MinimumHeight = dto.MinimumHeight,
            MayGetWet = dto.MayGetWet
        };
    }

    public static EntityChild ToDomain(this EntityChildDto dto)
    {
        return new EntityChild
        {
            Id = dto.Id,
            Name = dto.Name,
            EntityType = dto.EntityType.ToDomain(),
            ParentId = dto.ParentId,
            Location = dto.Location?.ToDomain()
        };
    }

    public static Location ToDomain(this EntityLocationDto dto)
    {
        return new Location
        {
            Latitude = dto.Latitude ?? 0,
            Longitude = dto.Longitude ?? 0
        };
    }

    public static Schedule ToDomain(this ScheduleDto dto)
    {
        return new Schedule
        {
            Id = dto.Id,
            Name = dto.Name,
            Timezone = dto.Timezone,
            Entries = dto.Schedule?.Select(e => e.ToDomain()).ToList() ?? new List<ScheduleEntry>()
        };
    }

    public static ScheduleEntry ToDomain(this EntityScheduleEntryDto dto)
    {
        return new ScheduleEntry
        {
            Date = dto.Date,
            Type = dto.Type,
            Description = dto.Description,
            OpeningTime = dto.OpeningTime,
            ClosingTime = dto.ClosingTime
        };
    }

    private static CoreEnums.LiveStatus ToDomain(this LiveStatusType dto)
    {
        return dto switch
        {
            LiveStatusType.OPERATING     => CoreEnums.LiveStatus.Operating,
            LiveStatusType.DOWN          => CoreEnums.LiveStatus.Down,
            LiveStatusType.CLOSED        => CoreEnums.LiveStatus.Closed,
            LiveStatusType.REFURBISHMENT => CoreEnums.LiveStatus.Refurbishment,
            _                            => CoreEnums.LiveStatus.Closed
        };
    }

    private static CoreEnums.EntityType ToDomain(this EntityType dto)
    {
        return dto switch
        {
            EntityType.DESTINATION => CoreEnums.EntityType.Destination,
            EntityType.PARK        => CoreEnums.EntityType.Park,
            EntityType.ATTRACTION  => CoreEnums.EntityType.Attraction,
            EntityType.RESTAURANT  => CoreEnums.EntityType.Restaurant,
            EntityType.HOTEL       => CoreEnums.EntityType.Hotel,
            EntityType.SHOW        => CoreEnums.EntityType.Show,
            _                      => CoreEnums.EntityType.Attraction
        };
    }

    private static CoreEnums.AttractionType ToDomain(this AttractionType dto)
    {
        return dto switch
        {
            AttractionType.RIDE       => CoreEnums.AttractionType.Ride,
            AttractionType.TRANSPORT  => CoreEnums.AttractionType.Transport,
            AttractionType.SHOW       => CoreEnums.AttractionType.Show,
            AttractionType.EXPERIENCE => CoreEnums.AttractionType.Experience,
            _                         => CoreEnums.AttractionType.Ride
        };
    }
}
