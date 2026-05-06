using HR.Domain.Employees;
using HR.Domain.Payrolls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.PayRolls
{
    public class PayrollEntryConfiguration : IEntityTypeConfiguration<PayrollEntry>
    {
        public void Configure(EntityTypeBuilder<PayrollEntry> builder)
        {
            builder.ToTable("PayrollEntries", Schemas.HR);
            builder.HasKey(e => e.Id);

            //  تحديد نوع البيانات المالية بدقة (18 رقم، منهم 2 عشري)
            builder.Property(e => e.BasicSalary).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Incentives).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Allowances).HasColumnType("decimal(18,2)");
            builder.Property(e => e.InsuranceDeduction).HasColumnType("decimal(18,2)");
            builder.Property(e => e.TaxDeduction).HasColumnType("decimal(18,2)");
            builder.Property(e => e.LoanDeduction).HasColumnType("decimal(18,2)");
            builder.Property(e => e.InsurancePurchaseDeduction).HasColumnType("decimal(18,2)");
            builder.Property(e => e.PenaltyDeduction).HasColumnType("decimal(18,2)");

            // تجاهل الخصائص المحسوبة
            builder.Ignore(e => e.GrossSalary);
            builder.Ignore(e => e.TotalDeductions);
            builder.Ignore(e => e.TotalAdditions);
            builder.Ignore(e => e.NetSalary);

            // ─── العلاقات ───────────────────────────────────────
           
            builder.HasOne<Employee>() 
                .WithMany()
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // منع مسح موظف له سجلات رواتب

            // 2. علاقة الراتب بالتسويات اليدوية
            builder.HasMany(e => e.Adjustments)
                .WithOne(a => a.Entry)
                .HasForeignKey(a => a.EntryId)
                .OnDelete(DeleteBehavior.Cascade);

            // إخبار EF Core باستخدام الحقل الخاص
            builder.Navigation(e => e.Adjustments)
                .HasField("_adjustments")
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
