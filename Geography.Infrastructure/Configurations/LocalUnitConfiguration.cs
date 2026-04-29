using Geography.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace Geography.Infrastructure.Configurations
{
    public class LocalUnitConfiguration : IEntityTypeConfiguration<LocalUnit>
    {
        public void Configure(EntityTypeBuilder<LocalUnit> builder)
        {
            builder.ToTable("LocalUnits", Schemas.Geopraphy);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CityCenterId)
                .IsRequired();

            builder.HasOne(x => x.CityCenter)
                .WithMany(x => x.LocalUnits)
                .HasForeignKey(x => x.CityCenterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.CityCenterId);
        }
    }
}