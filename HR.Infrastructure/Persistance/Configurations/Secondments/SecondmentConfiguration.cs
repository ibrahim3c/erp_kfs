using HR.Domain.Secondments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Secondments
{
    public class SecondmentConfiguration : IEntityTypeConfiguration<Secondment>
    {
        public void Configure(EntityTypeBuilder<Secondment> builder)
        {
            builder.ToTable("Secondments", Schemas.HR);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.HostEntityName).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.SalaryBearer).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.IncentiveBearer).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);

            builder.HasIndex(x => x.EmployeeId);
            builder.HasIndex(x => x.Status);
        }
    }
}
