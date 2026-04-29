using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using Organization.Domain;

namespace Organization.Infrastructure.Configurations
{
    public class LeadershipPositionConfiguration : IEntityTypeConfiguration<LeadershipPosition>
    {
        public void Configure(EntityTypeBuilder<LeadershipPosition> builder)
        {
            builder.ToTable("LeadershipPositions", Schemas.Organization);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrgUnitId)
                .IsRequired();

            builder.Property(x => x.JobTitleId)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired();

            builder.HasOne(x => x.OrgUnit)
                .WithMany()
                .HasForeignKey(x => x.OrgUnitId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.OrgUnitId);
        }
    }
}