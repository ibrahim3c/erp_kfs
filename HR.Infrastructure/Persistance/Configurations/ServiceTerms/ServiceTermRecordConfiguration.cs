using HR.Domain.ServiceTerms.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.ServiceTerms
{
    public class ServiceTermRecordConfiguration : IEntityTypeConfiguration<ServiceTermRecord>
    {
        public void Configure(EntityTypeBuilder<ServiceTermRecord> builder)
        {
            builder.ToTable("ServiceTermRecords", Schemas.HR);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.PreviousEntityName).HasMaxLength(300).IsRequired();
            builder.Property(x => x.Type).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
            builder.Property(x => x.CommitteeDecisionNumber).HasMaxLength(100);
            builder.Property(x => x.RejectionReason).HasMaxLength(500);
            builder.Property(x => x.AttachmentPath).HasMaxLength(500);

            builder.Ignore(x => x.NetDuration);

            builder.HasIndex(x => x.EmployeeId);
            builder.HasIndex(x => x.Status);
        }
    }
}
