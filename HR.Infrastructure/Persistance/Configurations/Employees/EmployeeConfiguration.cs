using HR.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", Schemas.HR);

            builder.HasKey(e => e.Id);

            // Validations
            builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.NationalId).IsRequired().HasMaxLength(14);
            builder.Property(e => e.Phone).HasMaxLength(20);

            builder.HasIndex(e => e.NationalId).IsUnique(); // الرقم القومي لا يتكرر
            builder.HasIndex(e => e.Code).IsUnique(); // كود الموظف لا يتكرر

            // --- العلاقات (Relationships) مع التوابع ---

            // 1. Employee Families
            builder.HasMany(e => e.Families)
                   .WithOne()
                   .HasForeignKey(f => f.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade); // مسح الموظف يمسح عائلته

            // إخبار EF Core بكيفية قراءة الـ Private List
            builder.Metadata.FindNavigation(nameof(Employee.Families))
                   ?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // 2. Employee Qualifications
            builder.HasMany(e => e.Qualifications)
                   .WithOne()
                   .HasForeignKey(q => q.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Employee.Qualifications))
                   ?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // 3. Employee Decisions
            builder.HasMany(e => e.Decisions)
                   .WithOne()
                   .HasForeignKey(d => d.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Metadata.FindNavigation(nameof(Employee.Decisions))
                   ?.SetPropertyAccessMode(PropertyAccessMode.Field);

            // 4. Leadership History (إذا تم إضافتها كما اتفقنا)
            // builder.HasMany(e => e.LeadershipHistory)
            //       .WithOne()
            //       .HasForeignKey(l => l.EmployeeId)
            //       .OnDelete(DeleteBehavior.Cascade);
            // builder.Metadata.FindNavigation(nameof(Employee.LeadershipHistory))
            //       ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
