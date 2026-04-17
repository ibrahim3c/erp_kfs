using HR.Domain.Payrolls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.PayRolls
{
    public class PayrollAdjustmentConfiguration : IEntityTypeConfiguration<PayrollAdjustment>
    {
        public void Configure(EntityTypeBuilder<PayrollAdjustment> builder)
        {
            builder.ToTable("PayrollAdjustments", Schemas.HR);
            builder.HasKey(a => a.Id);

           
            builder.Property(a => a.Type)
                .HasConversion<string>()
                .HasMaxLength(50);

            // الدقة المالية
            builder.Property(a => a.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(a => a.Reason)
                .HasMaxLength(300); 

            
        }
    }
}
