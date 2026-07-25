using HR.Domain.Retirement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Retriement
{
    public class RetirementFileConfiguration : IEntityTypeConfiguration<RetirementFile>
    {
        public void Configure(EntityTypeBuilder<RetirementFile> builder)
        {
            builder.ToTable("RetirementFiles", Schemas.HR);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.ReferralDate).IsRequired();
            builder.Property(x => x.Reason).HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.Stage).HasConversion<string>().HasMaxLength(50);
            builder.Property(x => x.Notes).HasMaxLength(1000);

            builder.HasIndex(x => x.EmployeeId);

            builder.OwnsMany(x => x.SalaryRecords, sr =>
            {
                sr.ToTable("RetirementSalaryRecords", Schemas.HR);
                sr.WithOwner().HasForeignKey("RetirementFileId");
                sr.Property<int>("Id").ValueGeneratedOnAdd();
                sr.HasKey("Id");
                sr.Property(x => x.Year).IsRequired();
                sr.Property(x => x.BasicInsuredSalary).HasColumnType("decimal(18,2)");
            });

            builder.Navigation(x => x.SalaryRecords).UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}

