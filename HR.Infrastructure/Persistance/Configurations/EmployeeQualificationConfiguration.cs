using HR.Domain.Employees;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HR.Infrastructure.Persistance.Configurations
{
    public class EmployeeQualificationConfiguration : IEntityTypeConfiguration<EmployeeQualification>
    {
        public void Configure(EntityTypeBuilder<EmployeeQualification> builder)
        {
            builder.ToTable("EmployeeQualifications", Schemas.HR);

            builder.HasKey(q => q.Id);

            builder.Property(q => q.QualificationFullName).IsRequired().HasMaxLength(200);
            builder.Property(q => q.University).HasMaxLength(150);
        }
    }
}
