using HR.Domain.Employees.Qualifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    internal class QualificationTypeConfiguration : IEntityTypeConfiguration<QualificationType>
    {
        public void Configure(EntityTypeBuilder<QualificationType> builder)
        {
            // 1. Table Name
            builder.ToTable("QualificationTypes",Schemas.HR);

            // 2. Primary Key
            builder.HasKey(q => q.Id);

            // 3. Properties

            builder.Property(q => q.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(q => q.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(q => q.IsActive)
                .IsRequired();

            // 4. Index

            builder.HasIndex(q => q.Name)
                .IsUnique(); // Prevent duplicate qualification types
        }
    }
}
