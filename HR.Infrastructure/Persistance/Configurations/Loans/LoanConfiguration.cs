using HR.Domain.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Loans
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loans", Schemas.HR);

            builder.HasKey(x => x.Id);
            builder.Property(l => l.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.InstallmentAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.RemainingAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // إعدادات النصوص والتواريخ
            builder.Property(l => l.Reason)
                .HasMaxLength(200);

            builder.Property(l => l.Months)
                .IsRequired();

            builder.Property(l => l.StartDate)
                .IsRequired();

            builder.Property(l => l.IsCompleted)
                .IsRequired();

            // إعداد العلاقة: الموظف والسلفة
            builder.HasOne(l => l.Employee)           
                   .WithMany(e => e.Loans)            
                   .HasForeignKey(l => l.EmployeeId)  
                   .OnDelete(DeleteBehavior.Restrict); // منع مسح الموظف إذا كان عليه سلف مسجلة
          
            builder.HasMany(l => l.Installments)    
                   .WithOne(i => i.Loan)             
                   .HasForeignKey(i => i.LoanId)     
                   .OnDelete(DeleteBehavior.Cascade);  // حذف الأقساط تلقائياً عند حذف السلفة
        }

    }
}
