using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using Organization.Domain;

namespace Organization.Infrastructure.Configurations
{
    public class OrgUnitTypeConfiguration : IEntityTypeConfiguration<OrgUnitType>
    {
        public void Configure(EntityTypeBuilder<OrgUnitType> builder)
        {
            builder.ToTable("OrgUnitTypes", Schemas.Organization);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.LevelOrder)
                .IsRequired();

            builder.Property(x => x.CanHaveChild)
                .IsRequired();

            builder.HasIndex(x => x.Code)
                .IsUnique();
        }
    }
}