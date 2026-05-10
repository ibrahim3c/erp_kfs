using HR.Domain.Decisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Decisions
{
    internal sealed class DecisionTypeConfiguration : IEntityTypeConfiguration<DecisionType>
    {
        public void Configure(EntityTypeBuilder<DecisionType> builder)
        {
            // 1. Table Name
            builder.ToTable("DecisionTypes", Schemas.HR);

            // 2. Primary Key
            builder.HasKey(d => d.Id);

            // 3. Properties

            builder.Property(d => d.Code)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(d => d.AffectsEmploymentType)
                .IsRequired();

            builder.Property(d => d.AffectsSalary)
                .IsRequired();

            builder.Property(d => d.AffectsPosition)
                .IsRequired();

            builder.Property(d => d.HasEndDate)
                .IsRequired();

            builder.Property(d => d.IsActive)
                .IsRequired();

            // 4. Indexes

            builder.HasIndex(d => d.Code)
                .IsUnique(); // مهم جدًا عشان ميتكررش نفس نوع القرار

            builder.HasIndex(d => d.IsActive);
        }

    }
}
