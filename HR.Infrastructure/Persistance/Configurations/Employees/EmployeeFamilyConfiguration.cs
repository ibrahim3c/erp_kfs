using HR.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    public class EmployeeFamilyConfiguration : IEntityTypeConfiguration<EmployeeFamily>
    {
        public void Configure(EntityTypeBuilder<EmployeeFamily> builder)
        {
            // 1. Table Name and Schema
            builder.ToTable("EmployeeFamilies", Schemas.HR);

            // 2. Primary Key
            builder.HasKey(f => f.Id);

            // 3. Properties

            builder.Property(f => f.FullName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(f => f.RelationshipType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(f => f.HealthStatus)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(f => f.NationalId)
                .HasMaxLength(14)
                .IsRequired(false);

            builder.Property(f => f.Phone)
                .HasMaxLength(20)
                .IsRequired(false);

            builder.Property(f => f.IsDisabled)
                .IsRequired();

            // 4. Foreign Key
            builder.Property(f => f.EmployeeId)
                .IsRequired();

            // 5. Relationships
            builder.HasOne<Employee>()
                .WithMany() // if Employee doesn't expose a navigation property
                .HasForeignKey(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 6. Indexes
            builder.HasIndex(f => f.EmployeeId);
            builder.HasIndex(f => f.NationalId);
        }
    }
}
