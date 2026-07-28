using HR.Domain.Legal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Legal
{
    internal sealed class CourtRulingConfiguration : IEntityTypeConfiguration<CourtRuling>
    {
        public void Configure(EntityTypeBuilder<CourtRuling> builder)
        {
            builder.ToTable("CourtRulings", Schemas.HR);

            builder.HasKey(cr => cr.Id);

            builder.Property(cr => cr.CaseNumber)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(cr => cr.Year)
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(cr => cr.EmployeeName)
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(cr => cr.Summary)
                   .HasMaxLength(2000)
                   .IsRequired();

            builder.Property(cr => cr.ExecutionType)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(cr => cr.AttachmentPath)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(cr => cr.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(cr => cr.CreatedAt)
                   .IsRequired();

            builder.Property(cr => cr.ExecutedAt)
                   .IsRequired(false);

            builder.HasIndex(cr => cr.CaseNumber);
            builder.HasIndex(cr => cr.EmployeeId);
            builder.HasIndex(cr => cr.Status);
        }
    }
}
