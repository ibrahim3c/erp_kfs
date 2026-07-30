using HR.Domain.Transfers.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Transefers
{
    public class ExternalMovementConfiguration : IEntityTypeConfiguration<ExternalMovement>
    {
        public void Configure(EntityTypeBuilder<ExternalMovement> builder)
        {
            builder.ToTable("ExternalMovements", Schemas.HR);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OtherEntityName).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.Direction).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.SalaryBearer).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.AttachmentPath).HasMaxLength(500);

            builder.HasIndex(x => x.EmployeeId);
        }
    }
}
