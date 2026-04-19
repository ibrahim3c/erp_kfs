using HR.Domain.Decisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Decisions
{
    internal sealed class DecisionConfiguration : IEntityTypeConfiguration<Decision>
    {
        public void Configure(EntityTypeBuilder<Decision> builder)
        {
            // 1. Table Name
            builder.ToTable("Decisions",Schemas.HR);

            // 2. Primary Key
            builder.HasKey(d => d.Id);

            // 3. Properties

            builder.Property(d => d.Number)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(d => d.Subject)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(d => d.Notes)
                .HasMaxLength(2000)
                .IsRequired(false);

            builder.Property(d => d.FilePath)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(d => d.DecisionDate)
                .IsRequired();

            builder.Property(d => d.ValidFrom)
                .IsRequired(false);

            builder.Property(d => d.ValidTo)
                .IsRequired(false);

            builder.Property(d => d.AffectsEmployee)
                .IsRequired();

            builder.Property(d => d.AffectsGroup)
                .IsRequired();

            builder.Property(d => d.IsTemporary)
                .IsRequired();

            // 4. Enum Mapping
            builder.Property(d => d.Status)
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            // 5. Foreign Keys

            builder.Property(d => d.DecisionTypeId)
                .IsRequired();

            builder.Property(d => d.DecisionAuthorityId)
                .IsRequired();

            // 6. Relationships

            // DecisionType (Many Decisions → One Type)
            builder.HasOne(d => d.DecisionType)
                .WithMany()
                .HasForeignKey(d => d.DecisionTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // DecisionAuthority (Many Decisions → One Authority)
            builder.HasOne(d => d.DecisionAuthority)
                .WithMany(du=>du.Decisions)
                .HasForeignKey(d => d.DecisionAuthorityId)
                .OnDelete(DeleteBehavior.Restrict);

            // 7. Indexes

            builder.HasIndex(d => d.Number)
                .IsUnique();
            builder.HasIndex(d => d.DecisionTypeId);
            builder.HasIndex(d => d.DecisionAuthorityId);

        }
    }
}
