using Organization.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace Organization.Infrastructure.Configurations
{
    public class FunctionalGroupConfiguration : IEntityTypeConfiguration<FunctionalGroup>
    {
        public void Configure(EntityTypeBuilder<FunctionalGroup> builder)
        {
            builder.ToTable("FunctionalGroups", Schemas.Organization);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.HasOne(x => x.QualitativeGroup)
                .WithMany()
                .HasForeignKey(x => x.QualitativeGroupId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}