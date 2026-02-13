using Market.Domain.Entities.Fakture;

namespace Market.Infrastructure.Database.Configurations.Fakture;

public sealed class FakturaStavkaConfigutation : IEntityTypeConfiguration<FakturaStavkaEntity>
{
    public void Configure(EntityTypeBuilder<FakturaStavkaEntity> builder)
    {
        builder.ToTable("FakturaStavke");
    }
}
