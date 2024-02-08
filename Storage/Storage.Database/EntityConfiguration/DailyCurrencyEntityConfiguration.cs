using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Storage.Database.Entities;

namespace Storage.Database.EntityConfiguration;

public class DailyCurrencyEntityConfiguration : IEntityTypeConfiguration<DailyCurrencyEntity>
{
    public void Configure(EntityTypeBuilder<DailyCurrencyEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.BaseCurrency)
            .WithMany(x => x.BaseDailyCurrencyEntities)
            .HasForeignKey(x => x.BaseCurrencyId);

        builder.HasOne(x => x.Currency)
            .WithMany(x => x.DailyCurrencyEntities)
            .HasForeignKey(x => x.CurrencyId);
    }
}
