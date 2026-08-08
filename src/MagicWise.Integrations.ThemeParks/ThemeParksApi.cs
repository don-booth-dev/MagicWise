using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MagicWise.Integrations.ThemeParks.Models;

namespace MagicWise.Integrations.ThemeParks
{
    /// <summary>
    /// Simple client for themeparks.wiki v1 API.
    /// Inject an HttpClient (configured via IHttpClientFactory) with BaseAddress set to the API base URL.
    /// </summary>
    public class ThemeParksApi
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly ILogger<ThemeParksApi>? _logger;

        public ThemeParksApi(HttpClient httpClient, ILogger<ThemeParksApi>? logger = null)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        private async Task<T?> GetAsync<T>(string relativeUri, CancellationToken ct = default)
        {
            try
            {
                using var resp = await _httpClient.GetAsync(relativeUri, ct).ConfigureAwait(false);

                if (!resp.IsSuccessStatusCode)
                {
                    var txt = await resp.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                    _logger?.LogWarning("ThemeParksApi GET {Uri} returned {StatusCode}: {Body}", relativeUri, resp.StatusCode, txt);
                    resp.EnsureSuccessStatusCode();
                }

                await using var stream = await resp.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
                if (stream == Stream.Null)
                    return default;

                var result = await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions, ct).ConfigureAwait(false);
                return result;
            }
            catch (OperationCanceledException) when (ct.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error calling ThemeParksApi GET {Uri}", relativeUri);
                throw;
            }
        }

        // GET /v1/destinations
        public Task<List<DestinationDto>?> GetDestinationsAsync(CancellationToken ct = default)
            => GetAsync<List<DestinationDto>>("/v1/destinations", ct);

        // GET /v1/entity/{id}
        public Task<EntityDto?> GetEntityAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id is required", nameof(id));
            var safe = Uri.EscapeDataString(id);
            return GetAsync<EntityDto?>($"/v1/entity/{safe}", ct);
        }

        // GET /v1/entity/{id}/children
        public Task<List<EntityDto>?> GetEntityChildrenAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id is required", nameof(id));
            var safe = Uri.EscapeDataString(id);
            return GetAsync<List<EntityDto>>($"/v1/entity/{safe}/children", ct);
        }

        // GET /v1/entity/{id}/live
        public Task<LiveStatusDto?> GetEntityLiveAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id is required", nameof(id));
            var safe = Uri.EscapeDataString(id);
            return GetAsync<LiveStatusDto?>($"/v1/entity/{safe}/live", ct);
        }

        // GET /v1/entity/{id}/schedule
        public Task<ScheduleDto?> GetEntityScheduleAsync(string id, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id is required", nameof(id));
            var safe = Uri.EscapeDataString(id);
            return GetAsync<ScheduleDto?>($"/v1/entity/{safe}/schedule", ct);
        }

        // GET /v1/entity/{id}/schedule/{year}/{month}
        public Task<ScheduleDto?> GetEntityScheduleAsync(string id, int year, int month, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new ArgumentException("id is required", nameof(id));
            if (month < 1 || month > 12) throw new ArgumentOutOfRangeException(nameof(month));
            var safe = Uri.EscapeDataString(id);
            return GetAsync<ScheduleDto?>($"/v1/entity/{safe}/schedule/{year}/{month}", ct);
        }
    }
}
