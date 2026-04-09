using HR.Domain.Employees;
using HR.Domain.Employees.Incentives;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HR.Infrastructure.Persistance.Configurations
{
    public class AcademicIncentiveRequestConfiguration : IEntityTypeConfiguration<AcademicIncentiveRequest>
    {
        public void Configure(EntityTypeBuilder<AcademicIncentiveRequest> builder)
        {
            // تحديد اسم الجدول والـ Schema
            builder.ToTable("AcademicIncentiveRequests",Schemas.HR);

            // المفتاح الأساسي
            builder.HasKey(a => a.Id);


            // القيود
            builder.Property(a => a.FilePath)
                   .IsRequired()
                   .HasMaxLength(500); // مسار الملف لا يجب أن يكون طويلاً جداً

            builder.Property(a => a.Notes)
                   .HasMaxLength(1000);

            builder.Property(a => a.Status)
                   .IsRequired(); // سيتم حفظه كأرقام (Int) بناءً على الـ Enum

            // إعداد العلاقة مع الموظف (One-to-Many)
            builder.HasOne<Employee>()
                   .WithMany(e => e.AcademicIncentiveRequests)
                   .HasForeignKey(a => a.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade); // إذا تم حذف الموظف، تُحذف طلباته

            // إخبار EF Core بكيفية قراءة القائمة المخفية داخل الموظف
            builder.Metadata.FindNavigation(nameof(Employee.AcademicIncentiveRequests))
                   ?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // ملاحظة: الحقول AcademicIncentiveTypeId و QualificationId سيتم التعامل معها 
            // كـ Foreign Keys لجداول أخرى في الداتا بيز إذا تم عمل Configurations لها لاحقاً.
        }
    }
}
