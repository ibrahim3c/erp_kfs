using HR.Domain.Employees;
using HR.Domain.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations
{
    public class LeadershipPositionHistoryConfiguration : IEntityTypeConfiguration<LeadershipPositionHistory>
    {
        public void Configure(EntityTypeBuilder<LeadershipPositionHistory> builder)
        {
            builder.ToTable("LeadershipPositionHistories", Schemas.HR);

            builder.HasKey(l => l.Id);

            // القيود (Validations)
            builder.Property(l => l.DecisionNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(l => l.Notes)
                   .HasMaxLength(1000); // الحقل اختياري ولكن نضع له حد أقصى

            // العلاقة مع الموظف
            builder.HasOne<Employee>()
                   .WithMany(e => e.LeadershipHistory) // تأكد من وجود هذه الـ List في كلاس Employee
                   .HasForeignKey(l => l.EmployeeId)
                   .OnDelete(DeleteBehavior.Cascade); // إذا تم حذف الموظف، يُحذف سجله القيادي
        }
    }
}
