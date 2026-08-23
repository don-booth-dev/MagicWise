using MagicWise.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MagicWise.Integrations.V1;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the themeparks.wiki HTTP client against <see cref="IThemeParksAPI"/>.
    /// </summary>
    public static IServiceCollection AddThemeParksIntegration(this IServiceCollection services)
    {
        services.AddHttpClient<IThemeParksAPI, ThemeParksAPI>(client =>
        {
            client.BaseAddress = new Uri("https://api.themeparks.wiki");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MagicWise/1.0");
        });

        return services;
    }
}
