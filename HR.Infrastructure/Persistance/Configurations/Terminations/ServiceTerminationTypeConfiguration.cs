
using HR.Domain.Terminations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Terminations
{
    public class ServiceTerminationTypeConfiguration : IEntityTypeConfiguration<ServiceTerminationType>
    {
        public void Configure(EntityTypeBuilder<ServiceTerminationType> builder)
        {
            builder.ToTable("ServiceTerminationTypes",Schemas.HR);

            builder.HasKey(t => t.Id);

            builder.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Description is optional by default for strings, but you can explicitly state it if needed.
             builder.Property(t => t.Description).HasMaxLength(500); 

        //    // Relationships
        //    builder.HasMany(t => t.ServiceTerminationRequests)
        //        .WithOne(r => r.ServiceTerminationType)
        //        .HasForeignKey(r => r.ServiceTerminationTypeId)
        //        .OnDelete(DeleteBehavior.Restrict); // Restrict deletion to prevent deleting a type if requests use it
        //
        }
    }
}
