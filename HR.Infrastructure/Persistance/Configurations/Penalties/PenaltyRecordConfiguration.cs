using HR.Domain.Penalties;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Penalties
{
    public class PenaltyRecordConfiguration : IEntityTypeConfiguration<PenaltyRecord>
    {
        public void Configure(EntityTypeBuilder<PenaltyRecord> builder)
        {
            builder.ToTable("PenaltyRecords", Schemas.HR);
            builder.HasKey(p => p.Id);

            builder.Property(p => p.DecisionReference).HasMaxLength(100);
            builder.Property(p => p.Notes).HasMaxLength(500);
            builder.Property(p => p.AttachmentPath).HasMaxLength(500);
            builder.Property(p => p.ViolationDate).IsRequired();
            builder.Property(p => p.ExecutionMonth).IsRequired();
            builder.Property(p => p.ActionType).IsRequired();
            builder.Property(p => p.DeductionDays).HasColumnType("decimal(5,2)");

            // ─── العلاقات ───

            // 1. الربط مع الموظف
            builder.HasOne(p => p.Employee)
                .WithMany()
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
