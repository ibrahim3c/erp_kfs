using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using Organization.Domain;

namespace Organization.Infrastructure.Configurations
{
    public class LeadershipPositionHistoryConfiguration : IEntityTypeConfiguration<LeadershipPositionHistory>
    {
        public void Configure(EntityTypeBuilder<LeadershipPositionHistory> builder)
        {
            builder.ToTable("LeadershipPositionHistories", Schemas.Organization);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.LeadershipPositionId)
                .IsRequired();

            builder.Property(x => x.EmployeeId)
                .IsRequired();

            builder.Property(x => x.StartDate)
                .IsRequired();

            builder.Property(x => x.DecisionNumber)
                .HasMaxLength(100);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.HasOne(x => x.LeadershipPosition)
                .WithMany()
                .HasForeignKey(x => x.LeadershipPositionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.LeadershipPositionId);
            builder.HasIndex(x => x.EmployeeId);
        }
    }
}