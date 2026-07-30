using HR.Domain.Funds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Funds
{
    internal sealed class FundClaimConfiguration : IEntityTypeConfiguration<FundClaim>
    {
        public void Configure(EntityTypeBuilder<FundClaim> builder)
        {
            builder.ToTable("FundClaims", Schemas.HR);

            builder.HasKey(fc => fc.Id);

            builder.Property(fc => fc.ClaimType)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(fc => fc.EventDate)
                   .IsRequired();

            builder.Property(fc => fc.Amount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired(false);

            builder.Property(fc => fc.AttachmentPath)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(fc => fc.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(fc => fc.CommitteeNotes)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            builder.Property(fc => fc.PaymentOrderNumber)
                   .HasMaxLength(100)
                   .IsRequired(false);

            builder.Property(fc => fc.PaymentDate)
                   .IsRequired(false);

            builder.Property(fc => fc.CreatedAt)
                   .IsRequired();

            builder.Property(fc => fc.UpdatedAt)
                   .IsRequired(false);

            builder.HasOne(fc => fc.Employee)
                   .WithMany()
                   .HasForeignKey(fc => fc.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(fc => fc.EmployeeId);
            builder.HasIndex(fc => fc.Status);
            builder.HasIndex(fc => fc.ClaimType);
        }
    }
}
