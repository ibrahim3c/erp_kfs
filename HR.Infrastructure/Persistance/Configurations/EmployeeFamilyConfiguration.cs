using HR.Domain.Employees;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HR.Infrastructure.Persistance.Configurations
{
    public class EmployeeFamilyConfiguration : IEntityTypeConfiguration<EmployeeFamily>
    {
        public void Configure(EntityTypeBuilder<EmployeeFamily> builder)
        {
            builder.ToTable("EmployeeFamilies", Schemas.HR);

            builder.HasKey(f => f.Id);

            builder.Property(f => f.FullName).IsRequired().HasMaxLength(200);
            builder.Property(f => f.RelationshipType).IsRequired().HasMaxLength(50);
            builder.Property(f => f.NationalId).HasMaxLength(14);
        }
    }
}
