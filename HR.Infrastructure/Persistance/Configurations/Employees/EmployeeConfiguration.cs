using HR.Domain.Employees;
using HR.Domain.JobStructures;
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

            builder.Property(e => e.Code)
                               .IsRequired()
                               .HasMaxLength(50);
            builder.HasIndex(e => e.Code).IsUnique(); // كود الموظف يجب أن يكون فريداً

            builder.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.NationalId)
                   .IsRequired()
                   .HasMaxLength(14);
            builder.HasIndex(e => e.NationalId).IsUnique(); // الرقم القومي يجب أن يكون فريداً

            builder.Property(e => e.Phone).HasMaxLength(20);
            builder.Property(e => e.Gender).HasMaxLength(20);
            builder.Property(e => e.Email).HasMaxLength(150);
            builder.Property(e => e.Address).HasMaxLength(500);
            builder.Property(e => e.MaritalStatus).HasMaxLength(50);
            //builder.Property(e => e.Specialization).HasMaxLength(200);

            builder.Property(e => e.IsActive)
                   .IsRequired();
            builder.Property(e => e.IsDisabled)
                .IsRequired();
            builder.Property(e => e.HireDate)
                .IsRequired();
            builder.Property(e => e.TerminationDate)
                .IsRequired(false);
            builder.Property(e => e.DateOfBirth)
                .IsRequired(false);

            //builder.HasOne<CityCenter>()   // from Geography module
            //  .WithMany()
            //  .HasForeignKey(e => e.CityCenterId)
            //  .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne<Village>()     // from Geography module
            //    .WithMany()
            //    .HasForeignKey(e => e.VillageId)
            //    .OnDelete(DeleteBehavior.Restrict);

            // 1. تعريفهم كحقول عادية (Soft References) بدون HasOne و WithMany
            builder.Property(e => e.CityCenterId).IsRequired(false);
            builder.Property(e => e.VillageId).IsRequired(false);
            builder.Property(e => e.OrgUnitId).IsRequired(false);

            builder.HasOne<EmploymentType>()
                .WithMany()
                .HasForeignKey(e => e.EmploymentTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<JobTitle>()
                .WithMany()
                .HasForeignKey(e => e.JobTitleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<JobGrade>()
                .WithMany()
                .HasForeignKey(e => e.JobGradeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<FunctionalGroup>()
                .WithMany()
                .HasForeignKey(e => e.FunctionalGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.HasOne<OrgUnit>()
            //    .WithMany()
            //    .HasForeignKey(e => e.OrgUnitId)
            //    .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(e => e.Code).IsUnique();
            builder.HasIndex(e => e.NationalId).IsUnique();

        }
    }
    }
