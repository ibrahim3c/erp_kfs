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
    internal sealed class PermissionRequestConfiguration
         : IEntityTypeConfiguration<PermissionRequest>
    {
        public void Configure(EntityTypeBuilder<PermissionRequest> builder)
        {
            builder.ToTable("PermissionRequests",Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId)
                .IsRequired();

            builder.Property(x => x.PermissionType)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.FromTime)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(x => x.ToTime)
                .IsRequired()
                .HasColumnType("time");

            builder.Property(x => x.DurationMinutes)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // ─── Relations ──────────────────────────────────────
            builder.HasOne(x => x.Employee)
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ─── Indexes ────────────────────────────────────────
            // بنبحث كتير بالموظف والشهر لحساب الحد الشهري
            builder.HasIndex(x => new { x.EmployeeId, x.Date });
        }
    }
}
