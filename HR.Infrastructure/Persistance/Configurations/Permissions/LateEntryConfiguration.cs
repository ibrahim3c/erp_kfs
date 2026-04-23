using HR.Domain.Permissions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Permissions
{
    internal sealed class LateEntryConfiguration
        : IEntityTypeConfiguration<LateEntry>
    {
        public void Configure(EntityTypeBuilder<LateEntry> builder)
        {
            builder.ToTable("LateEntries", Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.ActualArrivalTime)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(x => x.LateMinutes)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.IsTransferredToPenalty)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // ─── Relations ──────────────────────────────────────
            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ─── Indexes ────────────────────────────────────────
            // بنبحث بالموظف والشهر لحساب إجمالي دقائق التأخير
            builder.HasIndex(x => new { x.EmployeeId, x.Date });

            // بنفلتر على التأخيرات اللي لسه مش محولة لجزاء
            builder.HasIndex(x => new { x.EmployeeId, x.IsTransferredToPenalty });
        }
    }
}
