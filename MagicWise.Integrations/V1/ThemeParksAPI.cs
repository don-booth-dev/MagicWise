using MagicWise.Core.Interfaces;
using MagicWise.Core.Models;
using MagicWise.Integrations.V1.Models;
using MagicWise.Integrations.V1.Models.Responses;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MagicWise.Integrations.V1;

public class ThemeParksAPI : IThemeParksAPI
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public ThemeParksAPI(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    // GET /v1/destinations
    public async Task<List<Destination>?> GetDestinationsAsync(CancellationToken ct = default)
    {
        var dtos = await GetAsync<List<DestinationDto>>("/v1/destinations", ct).ConfigureAwait(false);
        return dtos?.Select(d => d.ToDomain()).ToList();
    }

    // GET /v1/entity/{id}
    public async Task<Entity?> GetEntityAsync(string id, CancellationToken ct = default)
    {
        string path = $"/v1/entity/{Uri.EscapeDataString(id)}";
        var dto = await GetAsync<EntityDto>(path, ct).ConfigureAwait(false);
        return dto?.ToDomain();
    }

    // GET /v1/entity/{id}/children
    public async Task<List<EntityChild>?> GetEntityChildrenAsync(string id, CancellationToken ct = default)
    {
        string path = $"/v1/entity/{Uri.EscapeDataString(id)}/children";
        var dtos = await GetAsync<List<EntityChildDto>>(path, ct).ConfigureAwait(false);
        return dtos?.Select(d => d.ToDomain()).ToList();
    }

    // GET /v1/entity/{id}/live
    public async Task<LiveData?> GetEntityLiveAsync(string id, CancellationToken ct = default)
    {
        string path = $"/v1/entity/{Uri.EscapeDataString(id)}/live";
        var dto = await GetAsync<EntityLiveDataDto>(path, ct).ConfigureAwait(false);
        return dto?.ToDomain();
    }

    // GET /v1/entity/{id}/schedule
    public async Task<Schedule?> GetEntityScheduleAsync(string id, CancellationToken ct = default)
    {
        string path = $"/v1/entity/{Uri.EscapeDataString(id)}/schedule";
        var dto = await GetAsync<ScheduleDto>(path, ct).ConfigureAwait(false);
        return dto?.ToDomain();
    }

    // GET /v1/entity/{id}/schedule/{year}/{month}
    public async Task<Schedule?> GetEntityScheduleAsync(string id, int year, int month, CancellationToken ct = default)
    {
        string path = $"/v1/entity/{Uri.EscapeDataString(id)}/schedule/{year}/{month}";
        var dto = await GetAsync<ScheduleDto>(path, ct).ConfigureAwait(false);
        return dto?.ToDomain();
    }

    private async Task<T?> GetAsync<T>(string relativePath, CancellationToken ct = default)
    {
        using var response = await _httpClient.GetAsync(relativePath, ct).ConfigureAwait(false);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return default;
        }

        // Special handling for 429 (rate limit) to populate RateLimitExceededErrorDto and RetryAfter
        if (response.StatusCode == (HttpStatusCode)429)
        {
            RateLimitExceededErrorDto? error = null;

            try
            {
                await using var errorStream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
                error = await JsonSerializer.DeserializeAsync<RateLimitExceededErrorDto>(errorStream, _jsonOptions, ct).ConfigureAwait(false);
            }
            catch
            {
                // ignore deserialization errors
            }

            // Prefer the explicit header if present
            if (response.Headers.RetryAfter != null)
            {
                var retry = response.Headers.RetryAfter;
                int? retrySeconds = null;

                if (retry.Delta.HasValue)
                    retrySeconds = (int)Math.Max(0, retry.Delta.Value.TotalSeconds);
                else if (retry.Date.HasValue)
                    retrySeconds = (int)Math.Max(0, (retry.Date.Value.UtcDateTime - DateTime.UtcNow).TotalSeconds);

                if (retrySeconds.HasValue)
                {
                    error ??= new RateLimitExceededErrorDto();
                    error.RetryAfter = retrySeconds.Value;
                }
            }

            // If the body provided a retryAfter field, keep it. If neither provided, leave null.
            string message = error?.Message ?? "Rate limit exceeded.";
            throw new ApiException(message, response.StatusCode, error);
        }

        if (!response.IsSuccessStatusCode)
        {
            ApiErrorDto? error = null;

            try
            {
                await using var errorStream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
                error = await JsonSerializer.DeserializeAsync<ApiErrorDto>(errorStream, _jsonOptions, ct).ConfigureAwait(false);
            }
            catch
            {
                // ignore
            }

            string message = error?.Message ?? $"Request failed with status {(int)response.StatusCode} ({response.ReasonPhrase}).";
            throw new ApiException(message, response.StatusCode, error);
        }

        await using var stream = await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
        using var document = await JsonDocument.ParseAsync(stream, cancellationToken: ct).ConfigureAwait(false);

        if (TryUnwrapPayload<T>(document.RootElement, out var unwrappedValue))
        {
            return unwrappedValue;
        }

        return document.RootElement.Deserialize<T>(_jsonOptions);
    }

    private static bool TryUnwrapPayload<T>(JsonElement root, out T? value)
    {
        value = default;

        if (typeof(T) == typeof(List<DestinationDto>))
        {
            if (root.TryGetProperty("destinations", out var destinations))
            {
                value = (T)(object?)destinations.Deserialize<List<DestinationDto>>(_jsonOptions)!;
                return true;
            }
        }

        if (typeof(T) == typeof(List<EntityChildDto>))
        {
            if (root.TryGetProperty("children", out var children))
            {
                value = (T)(object?)children.Deserialize<List<EntityChildDto>>(_jsonOptions)!;
                return true;
            }
        }

        if (typeof(T) == typeof(EntityLiveDataDto))
        {
            if (root.TryGetProperty("liveData", out var liveData))
            {
                var items = liveData.Deserialize<List<EntityLiveDataDto>>(_jsonOptions);
                var selectedItem = items?
                    .FirstOrDefault(x => root.TryGetProperty("id", out var idElement) && string.Equals(x.Id, idElement.GetString(), StringComparison.OrdinalIgnoreCase))
                    ?? items?.FirstOrDefault();

                value = (T?)(object?)selectedItem;
                return true;
            }
        }

        return false;
    }
}

