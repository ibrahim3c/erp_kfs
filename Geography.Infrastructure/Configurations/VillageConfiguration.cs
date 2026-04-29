using Geography.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace Geography.Infrastructure.Configurations
{
    public class VillageConfiguration : IEntityTypeConfiguration<Village>
    {
        public void Configure(EntityTypeBuilder<Village> builder)
        {
            builder.ToTable("Villages", Schemas.Geopraphy);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.LocalUnitId)
                .IsRequired();

            builder.HasOne(x => x.LocalUnit)
                .WithMany(x => x.Villages)
                .HasForeignKey(x => x.LocalUnitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.LocalUnitId);
        }
    }
}