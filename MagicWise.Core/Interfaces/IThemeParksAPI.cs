using MagicWise.Core.Models;

namespace MagicWise.Core.Interfaces;

public interface IThemeParksAPI
{
    // GET /v1/destinations
    Task<List<Destination>?> GetDestinationsAsync(CancellationToken ct = default);

    // GET /v1/entity/{id}
    Task<Entity?> GetEntityAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/children
    Task<List<EntityChild>?> GetEntityChildrenAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/live
    Task<LiveData?> GetEntityLiveAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/schedule
    Task<Schedule?> GetEntityScheduleAsync(string id, CancellationToken ct = default);

    // GET /v1/entity/{id}/schedule/{year}/{month}
    Task<Schedule?> GetEntityScheduleAsync(string id, int year, int month, CancellationToken ct = default);
}
