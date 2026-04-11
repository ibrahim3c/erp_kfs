using HR.Domain.Decisions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Decisions
{
    internal sealed class DecisionAuthorityConfiguration : IEntityTypeConfiguration<DecisionAuthority>
    {
        public void Configure(EntityTypeBuilder<DecisionAuthority> builder)
        {
            // 1. Table Name
            builder.ToTable("DecisionAuthorities",Schemas.HR);

            // 2. Primary Key
            builder.HasKey(d => d.Id);

            // 3. Properties

            builder.Property(d => d.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(d => d.Description)
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(d => d.IsActive)
                .IsRequired();

            // 4. Indexes

            builder.HasIndex(d => d.Name)
                .IsUnique(); // غالبًا اسم الجهة لا يتكرر

            builder.HasIndex(d => d.IsActive);
        }
    }

}
