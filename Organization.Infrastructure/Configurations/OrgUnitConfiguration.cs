using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using Organization.Domain;

namespace Organization.Infrastructure.Configurations
{
    public class OrgUnitConfiguration : IEntityTypeConfiguration<OrgUnit>
    {
        public void Configure(EntityTypeBuilder<OrgUnit> builder)
        {
            builder.ToTable("OrgUnits", Schemas.Organization);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.Property(x => x.OrgUnitTypeId)
                .IsRequired();

            builder.HasOne(x => x.OrgUnitType)
                .WithMany()
                .HasForeignKey(x => x.OrgUnitTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Parent)
                .WithMany(x => x.Children)
                .HasForeignKey(x => x.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Governorate)
                .WithMany()
                .HasForeignKey(x => x.GovernorateId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Code)
                .IsUnique();

            builder.HasIndex(x => x.Name);
        }
    }
}