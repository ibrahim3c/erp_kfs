using HR.Domain.Payrolls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;


namespace HR.Infrastructure.Persistance.Configurations.PayRolls
{
    public class PayrollCycleConfiguration : IEntityTypeConfiguration<PayrollCycle>
    {
        public void Configure(EntityTypeBuilder<PayrollCycle> builder)
        {
            builder.ToTable("PayrollCycles", Schemas.HR);
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Month).IsRequired();
            builder.Property(c => c.Year).IsRequired();

            // حفظ الـ Enum كنص في الداتا بيز (أفضل لقابلية القراءة وتجنب أخطاء الترتيب)
            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            
            builder.Ignore(c => c.EmployeeCount);
            builder.Ignore(c => c.TotalDeductions);
            builder.Ignore(c => c.TotalNetSalary);

            // ─── العلاقات ───────────────────────────────────────
            builder.HasMany(c => c.Entries)
                .WithOne() 
                .HasForeignKey(e => e.CycleId)
                .OnDelete(DeleteBehavior.Cascade); // عند حذف شهر الرواتب (مثلاً كمسودة)، تُحذف كل مفردات رواتب الموظفين لهذا الشهر

            builder.HasOne(c => c.EmploymentType)
                .WithMany() 
                .HasForeignKey(c => c.EmploymentTypeId)
                .OnDelete(DeleteBehavior.Restrict); // لا نريد حذف نوع التوظيف إذا تم حذف شهر الرواتب
           
            // إخبار EF Core باستخدام الحقل الخاص (Private Field) لتعبئة القائمة
            builder.Metadata.FindNavigation(nameof(PayrollCycle.Entries))
                ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
