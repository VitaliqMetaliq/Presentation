using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Storage.Database;

public static class DatabaseInitializer
{
    public static async Task InitAsync(IServiceProvider scopeServiceProvider)
    {
        var context = scopeServiceProvider.GetRequiredService<StorageDbContext>();
        await context.Database.MigrateAsync();
    }
}
