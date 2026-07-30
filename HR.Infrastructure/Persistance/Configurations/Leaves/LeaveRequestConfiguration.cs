using HR.Domain.Leaves;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Leaves
{
    internal sealed class LeaveRequestConfiguration : IEntityTypeConfiguration<LeaveRequest>
    {
        public void Configure(EntityTypeBuilder<LeaveRequest> builder)
        {
            builder.ToTable("LeaveRequests", Schemas.HR);

            builder.HasKey(lr => lr.Id);

            builder.Property(lr => lr.LeaveCategory)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(lr => lr.StartDate)
                   .IsRequired();

            builder.Property(lr => lr.EndDate)
                   .IsRequired();

            builder.Property(lr => lr.DurationDays)
                   .IsRequired();

            builder.Property(lr => lr.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(lr => lr.SalaryStatus)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(lr => lr.PayPercentage)
                   .HasColumnType("decimal(5,2)")
                   .IsRequired(false);

            builder.Property(lr => lr.ContactInfo)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(lr => lr.ReportAuthority)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(lr => lr.DecisionNumber)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(lr => lr.Diagnosis)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(lr => lr.ChildName)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(lr => lr.ChildDateOfBirth)
                   .IsRequired(false);

            builder.Property(lr => lr.AttachmentPath)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(lr => lr.Notes)
                   .HasMaxLength(1000)
                   .IsRequired(false);

            builder.Property(lr => lr.CreatedAt)
                   .IsRequired();

            builder.Property(lr => lr.ApprovedAt)
                   .IsRequired(false);

            builder.HasOne(lr => lr.Employee)
                   .WithMany()
                   .HasForeignKey(lr => lr.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(lr => lr.ReplacementEmployee)
                   .WithMany()
                   .HasForeignKey(lr => lr.ReplacementEmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(lr => lr.EmployeeId);
            builder.HasIndex(lr => lr.LeaveCategory);
            builder.HasIndex(lr => lr.Status);
            builder.HasIndex(lr => lr.StartDate);
        }
    }
}
