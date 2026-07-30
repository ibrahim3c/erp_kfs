using HR.Domain.Funds;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Funds
{
    internal sealed class FundSubscriptionConfiguration : IEntityTypeConfiguration<FundSubscription>
    {
        public void Configure(EntityTypeBuilder<FundSubscription> builder)
        {
            builder.ToTable("FundSubscriptions", Schemas.HR);

            builder.HasKey(fs => fs.Id);

            builder.Property(fs => fs.SubscriptionDate)
                   .IsRequired();

            builder.Property(fs => fs.FundType)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(fs => fs.DeductionAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(fs => fs.BankAgreement)
                   .IsRequired();

            builder.Property(fs => fs.Status)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(fs => fs.Notes)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(fs => fs.CreatedAt)
                   .IsRequired();

            builder.Property(fs => fs.UpdatedAt)
                   .IsRequired(false);

            builder.HasOne(fs => fs.Employee)
                   .WithMany()
                   .HasForeignKey(fs => fs.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(fs => fs.EmployeeId);
            builder.HasIndex(fs => fs.Status);
            builder.HasIndex(fs => fs.FundType);
        }
    }
}
