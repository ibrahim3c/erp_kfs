using HR.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using Organization.Domain;

namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees", Schemas.HR);
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Code).IsRequired().HasMaxLength(50);
            builder.HasIndex(e => e.Code).IsUnique();

            builder.Property(e => e.Name).IsRequired().HasMaxLength(200);
            builder.Property(e => e.NationalId).IsRequired().HasMaxLength(14);
            builder.HasIndex(e => e.NationalId).IsUnique();

            builder.Property(e => e.Phone).HasMaxLength(20);
            builder.Property(e => e.Gender).HasMaxLength(20);
            builder.Property(e => e.Email).HasMaxLength(150);
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.MaritalStatus).HasMaxLength(50);

            builder.Property(e => e.IsActive).IsRequired();
            builder.Property(e => e.IsDisabled).IsRequired();
            builder.Property(e => e.HireDate).IsRequired();
            builder.Property(e => e.TerminationDate).IsRequired(false);
            builder.Property(e => e.DateOfBirth).IsRequired(false);

            builder.Property(e => e.CityCenterId).IsRequired(false);
            builder.Property(e => e.VillageId).IsRequired(false);
            builder.Property(e => e.OrgUnitId).IsRequired(false);

            builder.Property(e => e.EmploymentTypeId).IsRequired(false);
            builder.Property(e => e.JobTitleId).IsRequired(false);
            builder.Property(e => e.JobGradeId).IsRequired(false);
            builder.Property(e => e.FunctionalGroupId).IsRequired(false);
            builder.Property(e => e.OrgUnitId).IsRequired(false);

           builder.Property(e => e.LeadershipPositionId).IsRequired(false);
        }
    }
}