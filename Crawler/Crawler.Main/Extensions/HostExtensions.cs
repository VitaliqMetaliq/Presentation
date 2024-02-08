using Crawler.Database;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Main.Extensions;

public static class HostExtensions
{
    public static async Task<IHost> InitDatabase(this IHost host)
    {
        IServiceProvider serviceProvider = host.Services;

        using (var scope = serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<HangfireDbContext>();

            await dbContext.Database.MigrateAsync();
        }

        return host;
    }
}

