using HR.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    internal sealed class EmployeeFinancialConfiguration : IEntityTypeConfiguration<EmployeeFinancial>
    {
        public void Configure(EntityTypeBuilder<EmployeeFinancial> builder)
        {
            // 1. Table Name
            builder.ToTable("EmployeeFinancials",Schemas.HR);

            // 2. Primary Key
            builder.HasKey(e => e.Id);

            // 3. Properties Configuration
            builder.Property(e => e.EmployeeId)
                .IsRequired();

            // ضبط دقة الأرقام للرواتب (18 رقم إجمالي، منهم 2 بعد العلامة العشرية)
            builder.Property(e => e.BasicSalary2019)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            builder.Property(e => e.GrossSalary)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);

            // تحديد أطوال النصوص (مهم جداً لتحسين أداء قاعدة البيانات)
            builder.Property(e => e.InsuranceNumber)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(e => e.BankName)
                .HasMaxLength(150)
                .IsRequired(false);

            builder.Property(e => e.BankAccount)
                .HasMaxLength(100)
                .IsRequired(false);

            // إعدادات القيم الافتراضية للصناديق
            builder.Property(e => e.HasFellowshipFund)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(e => e.HasMedicalFund)
                .IsRequired()
                .HasDefaultValue(false);

            // 4. Relationships
            // إعداد علاقة One-to-One مع كيان الموظف (Employee)
            // استخدام Cascade Delete يعني أنه إذا تم حذف الموظف، سيتم حذف بياناته المالية تلقائياً
            builder.HasOne<Employee>()
                .WithOne(e => e.FinancialInfo)
                .HasForeignKey<EmployeeFinancial>(e => e.EmployeeId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
