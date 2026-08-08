using MagicWise.Integrations.V1.Models;

namespace MagicWise.Integrations.V1.Interfaces;

internal interface IThemeParksAPI
{
    // GET /v1/destinations
    public Task<List<DestinationDto>?> GetDestinationsAsync(CancellationToken ct = default);

    // GET /v1/entity/{id}
    public Task<EntityDto?> GetEntityAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/children
    public Task<List<EntityDto>?> GetEntityChildrenAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/live
    public Task<EntityLiveDataDto?> GetEntityLiveAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/schedule
    public Task<ScheduleDto?> GetEntityScheduleAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/schedule/{year}/{month}
    public Task<ScheduleDto?> GetEntityScheduleAsync(string id, int year, int month, CancellationToken ct = default);
}
