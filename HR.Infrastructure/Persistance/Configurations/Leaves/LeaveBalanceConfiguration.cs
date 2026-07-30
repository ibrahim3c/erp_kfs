using HR.Domain.Leaves;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Leaves
{
    internal sealed class LeaveBalanceConfiguration : IEntityTypeConfiguration<LeaveBalance>
    {
        public void Configure(EntityTypeBuilder<LeaveBalance> builder)
        {
            builder.ToTable("LeaveBalances", Schemas.HR);

            builder.HasKey(lb => lb.Id);

            builder.Property(lb => lb.Year)
                   .IsRequired();

            builder.Property(lb => lb.RegularLeaveEntitled)
                   .IsRequired();

            builder.Property(lb => lb.RegularLeaveUsed)
                   .IsRequired();

            builder.Property(lb => lb.CasualLeaveEntitled)
                   .IsRequired();

            builder.Property(lb => lb.CasualLeaveUsed)
                   .IsRequired();

            builder.Property(lb => lb.CarryOverRegularDays)
                   .IsRequired();

            builder.Property(lb => lb.CreatedAt)
                   .IsRequired();

            builder.Property(lb => lb.UpdatedAt)
                   .IsRequired(false);

            builder.Ignore(lb => lb.RegularRemaining);
            builder.Ignore(lb => lb.CasualRemaining);

            builder.HasOne(lb => lb.Employee)
                   .WithMany()
                   .HasForeignKey(lb => lb.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(lb => new { lb.EmployeeId, lb.Year }).IsUnique();
        }
    }
}
