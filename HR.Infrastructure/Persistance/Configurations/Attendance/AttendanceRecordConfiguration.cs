using HR.Domain.Attendance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Attendance
{
    internal sealed class AttendanceRecordConfiguration
        : IEntityTypeConfiguration<AttendanceRecord>
    {
        public void Configure(EntityTypeBuilder<AttendanceRecord> builder)
        {
            builder.ToTable("AttendanceRecords", Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.CheckIn)
                .HasColumnType("time")
                .IsRequired(false);

            builder.Property(x => x.CheckOut)
                .HasColumnType("time")
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<int>();

            builder.Property(x => x.WorkedHours)
                .HasPrecision(5, 2)
                .HasDefaultValue(0);

            builder.Property(x => x.LateMinutes)
                .HasDefaultValue(0);

            builder.Property(x => x.Notes)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.LateEntryId)
                .IsRequired(false);

            builder.Property(x => x.PermissionRequestId)
                .IsRequired(false);

            // ─── Relations ──────────────────────────────────────
            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.LateEntry)
                .WithMany()
                .HasForeignKey(x => x.LateEntryId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(x => x.PermissionRequest)
                .WithMany()
                .HasForeignKey(x => x.PermissionRequestId)
                .OnDelete(DeleteBehavior.SetNull);

            // ─── Indexes ────────────────────────────────────────
            builder.HasIndex(x => new { x.EmployeeId, x.Date }).IsUnique();
            builder.HasIndex(x => x.Date);
            builder.HasIndex(x => x.Status);
        }
    }
}
