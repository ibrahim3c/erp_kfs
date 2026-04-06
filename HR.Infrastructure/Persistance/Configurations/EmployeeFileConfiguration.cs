using HR.Domain.Employees;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HR.Infrastructure.Persistance.Configurations
{
    public class EmployeeFileConfiguration : IEntityTypeConfiguration<EmployeeFile>
    {
        public void Configure(EntityTypeBuilder<EmployeeFile> builder)
        {
            builder.ToTable("EmployeeFiles", Schemas.HR);

            builder.HasKey(f => f.Id);

            builder.Property(f => f.MilitaryFile).HasMaxLength(255);
            builder.Property(f => f.QualificationFile).HasMaxLength(255);
            builder.Property(f => f.PersonalPhoto).HasMaxLength(255);

            // One-to-One Relationship with Employee
            builder.HasOne<Employee>()
                   .WithOne()
                   .HasForeignKey<EmployeeFile>(f => f.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
