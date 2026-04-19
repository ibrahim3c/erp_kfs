using HR.Domain.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Loans
{
    public sealed class InsurancePeriodPurchaseConfiguration
        : IEntityTypeConfiguration<InsurancePeriodPurchase>
    {
        public void Configure(EntityTypeBuilder<InsurancePeriodPurchase> builder)
        {
            builder.ToTable("InsurancePeriodPurchases",Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId)
                .IsRequired();

            builder.Property(x => x.InsuranceAuthority)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.PurchasedYears)
                .IsRequired();

            builder.Property(x => x.TotalCost)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.MonthlyInstallment)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.RemainingAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DeductionStartDate)
                .IsRequired();

            builder.Property(x => x.ApprovalDecisionFilePath)
                .IsRequired(false)
                .HasMaxLength(500);

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>()   // نخزنه كـ string مقروء في الـ DB
                .HasMaxLength(30);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.CompletedAt)
                .IsRequired(false);

            // ─── Ignored (Computed) ──────────────────────────────────
            // دي Computed Properties — مش هتتخزن في الـ DB
            builder.Ignore(x => x.IsActive);
            builder.Ignore(x => x.IsCompleted);
            builder.Ignore(x => x.EstimatedRemainingMonths);

            // ─── Relations ───────────────────────────────────────────
            builder.HasOne(x => x.Employee)
                .WithMany(e=>e.InsurancePeriodPurchases)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // ─── Indexes ─────────────────────────────────────────────
            builder.HasIndex(x => x.EmployeeId);

            // بنفلتر عليهم كتير بالـ Status (معتمد / منتهى)
            builder.HasIndex(x => new { x.EmployeeId, x.Status });
        }
    }
}
