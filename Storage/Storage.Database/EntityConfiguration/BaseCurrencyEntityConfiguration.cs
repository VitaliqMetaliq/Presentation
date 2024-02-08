using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Storage.Database.Entities;

namespace Storage.Database.EntityConfiguration;

public class BaseCurrencyEntityConfiguration : IEntityTypeConfiguration<BaseCurrencyEntity>
{
    public void Configure(EntityTypeBuilder<BaseCurrencyEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ParentCode).HasColumnName("RId");

        builder.Property(x => x.ISOCharCode).HasColumnName("IsoCode");
    }
}
