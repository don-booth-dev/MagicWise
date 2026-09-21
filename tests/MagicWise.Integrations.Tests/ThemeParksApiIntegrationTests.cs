using MagicWise.Integrations.V1;

namespace MagicWise.Integrations.Tests;

public class ThemeParksApiIntegrationTests
{
    private static ThemeParksAPI CreateApi()
    {
        return new ThemeParksAPI(new HttpClient
        {
            BaseAddress = new Uri("https://api.themeparks.wiki/"),
            Timeout = TimeSpan.FromSeconds(30)
        });
     }

    private static async Task<string> GetKnownParkIdAsync()
    {
        var api = CreateApi();
        var destinations = await api.GetDestinationsAsync(TestContext.Current.CancellationToken);

        var park = destinations?
            .SelectMany(destination => destination.Parks ?? [])
            .FirstOrDefault();

        Assert.NotNull(park);
        return park!.Id;
    }

    [Fact]
    public async Task GetDestinationsAsync_returns_destinations()
    {
        var api = CreateApi();

        var destinations = await api.GetDestinationsAsync(TestContext.Current.CancellationToken);

        Assert.NotNull(destinations);
        Assert.NotEmpty(destinations!);
        Assert.False(string.IsNullOrWhiteSpace(destinations![0].Id));
    }

    [Fact]
    public async Task GetEntityAsync_returns_entity_for_known_park()
    {
        var api = CreateApi();
        var parkId = await GetKnownParkIdAsync();

        var entity = await api.GetEntityAsync(parkId, TestContext.Current.CancellationToken);

        Assert.NotNull(entity);
        Assert.Equal(parkId, entity!.Id);
        Assert.False(string.IsNullOrWhiteSpace(entity.Name));
    }

    [Fact]
    public async Task GetEntityChildrenAsync_returns_children_for_known_park()
    {
        var api = CreateApi();
        var parkId = await GetKnownParkIdAsync();

        var children = await api.GetEntityChildrenAsync(parkId, TestContext.Current.CancellationToken);

        Assert.NotNull(children);
        Assert.NotEmpty(children!);
        Assert.False(string.IsNullOrWhiteSpace(children![0].Id));
    }

    [Fact]
    public async Task GetEntityLiveAsync_returns_live_data_for_known_park()
    {
        var api = CreateApi();
        var parkId = await GetKnownParkIdAsync();

        var liveStatus = await api.GetEntityLiveAsync(parkId, TestContext.Current.CancellationToken);

        Assert.NotNull(liveStatus);
        Assert.False(string.IsNullOrWhiteSpace(liveStatus!.Name));
    }

    [Fact]
    public async Task GetEntityScheduleAsync_returns_schedule_for_known_park()
    {
        var api = CreateApi();
        var parkId = await GetKnownParkIdAsync();

        var schedule = await api.GetEntityScheduleAsync(parkId, TestContext.Current.CancellationToken);

        Assert.NotNull(schedule);
        Assert.NotNull(schedule!.Entries);
        Assert.NotEmpty(schedule.Entries!);
    }

    [Fact]
    public async Task GetEntityScheduleAsync_with_year_and_month_returns_schedule_for_known_park()
    {
        var api = CreateApi();
        var parkId = await GetKnownParkIdAsync();

        var schedule = await api.GetEntityScheduleAsync(parkId, 2023, 10, TestContext.Current.CancellationToken);

        Assert.NotNull(schedule);
        Assert.NotNull(schedule!.Entries);
        Assert.NotEmpty(schedule.Entries!);
    }
}

