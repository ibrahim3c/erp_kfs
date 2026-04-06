using HR.Domain.Employees;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HR.Infrastructure.Persistance.Configurations
{
    public class EmployeeDecisionConfiguration : IEntityTypeConfiguration<EmployeeDecision>
    {
        public void Configure(EntityTypeBuilder<EmployeeDecision> builder)
        {
            // تحديد اسم الجدول والـ Schema
            builder.ToTable("EmployeeDecisions", Schemas.HR);

            // المفتاح الأساسي (موروث من Entity)
            builder.HasKey(d => d.Id);

            // القيود (Validations)
            builder.Property(d => d.Description)
                   .IsRequired()
                   .HasMaxLength(500); // تجنب nvarchar(max)

            builder.Property(d => d.DecisionId)
                   .IsRequired();

            // ملاحظة: علاقة (الموظف -> القرارات) تم تعريفها مسبقاً في EmployeeConfiguration
            // باستخدام Cascade Delete للحفاظ على نظافة قاعدة البيانات
        }
    }
}
