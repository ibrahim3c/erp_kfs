using HR.Domain.Evaluations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Evaluations
{
    internal sealed class GrievanceConfiguration : IEntityTypeConfiguration<Grievance>
    {
        public void Configure(EntityTypeBuilder<Grievance> builder)
        {
            builder.ToTable("Grievances", Schemas.HR);

            builder.HasKey(g => g.Id);

            builder.Property(g => g.ComplainedDecisionNumber)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(g => g.ComplainedDecisionDate)
                   .IsRequired();

            builder.Property(g => g.SubmissionDate)
                   .IsRequired();

            builder.Property(g => g.Reasons)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(g => g.AttachmentPath)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(g => g.GrievanceType)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(g => g.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(g => g.CommitteeNotes)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(g => g.ResolutionDate)
                   .IsRequired(false);

            builder.HasOne(g => g.Employee)
                   .WithMany()
                   .HasForeignKey(g => g.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(g => g.EmployeeId);
            builder.HasIndex(g => g.Status);
        }
    }
}
