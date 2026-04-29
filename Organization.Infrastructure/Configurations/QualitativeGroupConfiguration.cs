using Organization.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace Organization.Infrastructure.Configurations
{
    public class QualitativeGroupConfiguration : IEntityTypeConfiguration<QualitativeGroup>
    {
        public void Configure(EntityTypeBuilder<QualitativeGroup> builder)
        {
            builder.ToTable("QualitativeGroups", Schemas.Organization);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}