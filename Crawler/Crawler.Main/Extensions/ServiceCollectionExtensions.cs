using Crawler.Core.Settings;

namespace Crawler.Main.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void ConfigureSettings(
        this IServiceCollection services,
        IConfiguration configuration,
        out AppSettings appSettings)
    {
        appSettings = new();

        configuration.Bind(appSettings);
        services.Configure<ExternalUrlsSettings>(configuration.GetSection(nameof(ExternalUrlsSettings)));
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
    }
}

