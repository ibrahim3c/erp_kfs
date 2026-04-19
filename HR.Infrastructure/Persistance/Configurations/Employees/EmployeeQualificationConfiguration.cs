using HR.Domain.Employees.Qualifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    public class EmployeeQualificationConfiguration : IEntityTypeConfiguration<EmployeeQualification>
    {
        public void Configure(EntityTypeBuilder<EmployeeQualification> builder)
        {
            // 1. Table Name and Schema
            builder.ToTable("EmployeeQualifications", Schemas.HR);

            // 2. Primary Key
            builder.HasKey(q => q.Id);

            // 3. Properties

            builder.Property(q => q.QualificationFullName)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(q => q.Specialization)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(q => q.University)
                .HasMaxLength(200)
                .IsRequired(false);

            builder.Property(q => q.GraduationYear)
                .IsRequired(false);

            builder.Property(q => q.Grade)
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(q => q.FilePath)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(q => q.IsVerified)
                .IsRequired();

            builder.Property(q => q.ValidFrom)
                .IsRequired(false);

            builder.Property(q => q.ValidTo)
                .IsRequired(false);

            builder.Property(q => q.Notes)
                .HasMaxLength(1000)
                .IsRequired(false);

            // 4. Foreign Keys

            builder.Property(q => q.EmployeeId)
                .IsRequired();

            builder.Property(q => q.QualificationTypeId)
                .IsRequired();

            // 5. Relationships

            // Employee (Many Qualifications)
            builder.HasOne(q => q.Employee)
                .WithMany()
                .HasForeignKey(q => q.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            // QualificationType (Many Qualifications)
            builder.HasOne<QualificationType>()
                .WithMany()
                .HasForeignKey(q => q.QualificationTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 6. Indexes

            builder.HasIndex(q => q.EmployeeId);

            builder.HasIndex(q => q.QualificationTypeId);

            builder.HasIndex(q => new { q.EmployeeId, q.QualificationTypeId });
        }
    }
}
