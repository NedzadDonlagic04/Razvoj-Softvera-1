namespace Market.Infrastructure.Database.Configurations.Catalog;

public sealed class DostavljacConfiguration : IEntityTypeConfiguration<DostavljacEntity>
{
    public void Configure(EntityTypeBuilder<DostavljacEntity> builder)
    {
        builder.ToTable("Dostavljaci");

        builder.HasIndex(x => x.Code).HasDatabaseName("IX_Dostavljaci_Code");
        builder.Property(x => x.Code).HasMaxLength(DostavljacEntity.Constraints.CodeMaxLength);
    }
}
