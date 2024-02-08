using Storage.Core.Settings;

namespace Storage.Main.Extensions;

internal static class ServiceCollectionExtensions
{
    public static void ConfigureSettings(
        this IServiceCollection services,
        IConfiguration configuration,
        out AppSettings appSettings)
    {
        appSettings = new();

        configuration.Bind(appSettings);
        services.Configure<DbSettings>(configuration.GetSection(nameof(DbSettings)));
    }
}
