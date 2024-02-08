using Crawler.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crawler.Database;

public class HangfireDbContext : DbContext
{
    public DbSet<SavePointEntity> SavePoints { get; set; }

    public HangfireDbContext(DbContextOptions<HangfireDbContext> options) : base(options)
    {
    }
}

