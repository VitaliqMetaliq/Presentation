using Microsoft.EntityFrameworkCore;
using Storage.Database.Entities;
using Storage.Database.EntityConfiguration;

namespace Storage.Database;

public class StorageDbContext : DbContext
{
    public DbSet<BaseCurrencyEntity> BaseCurrencies { get; set; }
    public DbSet<DailyCurrencyEntity> DailyCurrencies { get; set; }

    public StorageDbContext(DbContextOptions<StorageDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BaseCurrencyEntityConfiguration());
        modelBuilder.ApplyConfiguration(new DailyCurrencyEntityConfiguration());
    }
}

