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
    public sealed class LoanInstallmentConfiguration : IEntityTypeConfiguration<LoanInstallment>
    {
        public void Configure(EntityTypeBuilder<LoanInstallment> builder)
        {
            builder.ToTable("LoanInstallments", Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.LoanId)
                .IsRequired();

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DueDate)
                .IsRequired();

            builder.Property(x => x.IsPaid)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(x => x.PaidAt)
                .IsRequired(false);

            // ─── Indexes ────────────────────────────────────────────
            // بنبحث كتير عن الأقساط غير المدفوعة لقسط معين
            builder.HasIndex(x => new { x.LoanId, x.IsPaid });

            // بنبحث عن الأقساط اللي استحقت في شهر معين (خدمة الرواتب)
            builder.HasIndex(x => x.DueDate);
        }
    }
}
