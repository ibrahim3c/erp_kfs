using Geography.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace Geography.Infrastructure.Configurations
{
    public class CityCenterConfiguration : IEntityTypeConfiguration<CityCenter>
    {
        public void Configure(EntityTypeBuilder<CityCenter> builder)
        {
            builder.ToTable("CityCenters", Schemas.Geopraphy);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.GovernorateId)
                .IsRequired();

            builder.HasOne(x => x.Governorate)
                .WithMany(x => x.CityCenters)
                .HasForeignKey(x => x.GovernorateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.Name);
            builder.HasIndex(x => x.GovernorateId);
        }
    }
}