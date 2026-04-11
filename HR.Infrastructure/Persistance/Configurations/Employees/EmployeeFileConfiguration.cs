using HR.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    public class EmployeeFileConfiguration : IEntityTypeConfiguration<EmployeeFile>
    {
        public void Configure(EntityTypeBuilder<EmployeeFile> builder)
        {
            // 1. Table Name and Schema
            builder.ToTable("EmployeeFiles", Schemas.HR);

            // 2. Primary Key
            builder.HasKey(f => f.Id);

            // 3. Properties

            builder.Property(f => f.MilitaryFile)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.QualificationFile)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.BirthCertificateFile)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.PoliceClearanceCertificate)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.NationalIdCardFront)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.NationalIdCardBack)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.MarriageDocument)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(f => f.PersonalPhoto)
                .HasMaxLength(500)
                .IsRequired(false);

            // 4. Foreign Key

            builder.Property(f => f.EmployeeId)
                .IsRequired();

            // 5. Relationship (One-to-One)

            builder.HasOne<Employee>()
                .WithOne() // assuming Employee does not expose navigation property
                .HasForeignKey<EmployeeFile>(f => f.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // 6. Index

            builder.HasIndex(f => f.EmployeeId)
                .IsUnique(); // ensures one file record per employee
        }
    }
}
