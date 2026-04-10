using HR.Domain.Organization;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HR.Infrastructure.Persistance.Configurations
{
    public class OrgUnitConfiguration : IEntityTypeConfiguration<OrgUnit>
    {
        public void Configure(EntityTypeBuilder<OrgUnit> builder)
        {
            builder.ToTable("OrgUnits", Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(150);

            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            // Self Reference (Parent / Children)

            builder.HasOne(x => x.Parent)
               .WithMany(x => x.Children)
               .HasForeignKey(x => x.ParentId)
               .OnDelete(DeleteBehavior.Restrict);

            // Relation with OrgUnitType
            builder.HasOne(x => x.OrgUnitType)
                   .WithMany() 
                   .HasForeignKey(x => x.OrgUnitTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexes (Important)
            builder.HasIndex(x => x.Code)
                   .IsUnique();

            builder.HasIndex(x => x.ParentId);

            builder.HasIndex(x => x.OrgUnitTypeId);
        }
    }
}
