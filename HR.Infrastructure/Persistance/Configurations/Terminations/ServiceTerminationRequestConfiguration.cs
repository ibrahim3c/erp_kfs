using HR.Domain.Employees.Termination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Terminations
{
    public class ServiceTerminationRequestConfiguration : IEntityTypeConfiguration<ServiceTerminationRequest>
    {
        public void Configure(EntityTypeBuilder<ServiceTerminationRequest> builder)
        {
            builder.ToTable("ServiceTerminationRequests",Schemas.HR);

            builder.HasKey(r => r.Id);

            builder.Property(r => r.RequestNumber)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.IssuedTo)
                .HasMaxLength(200);

            builder.Property(r => r.Status)
                .HasMaxLength(50);

            builder.Property(r => r.FilePath)
                .HasMaxLength(300);

            // Relationships
            // 1. Relationship with Employee
            builder.HasOne(r => r.Employee)
                .WithMany() // Assuming Employee doesn't have an ICollection<ServiceTerminationRequest>. If it does, put it inside WithMany(e => e.Requests)
                .HasForeignKey(r => r.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Relationship with ServiceTerminationType (Already configured in Type config, but good to ensure both sides align)
            builder.HasOne(r => r.ServiceTerminationType)
                .WithMany(t => t.ServiceTerminationRequests)
                .HasForeignKey(r => r.ServiceTerminationTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
