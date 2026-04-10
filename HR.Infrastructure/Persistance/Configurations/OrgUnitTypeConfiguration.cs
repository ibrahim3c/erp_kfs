using HR.Domain.Organization;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace HR.Infrastructure.Persistance.Configurations
{
    public class OrgUnitTypeConfiguration : IEntityTypeConfiguration<OrgUnitType>
    {
        public void Configure(EntityTypeBuilder<OrgUnitType> builder)
        {
            builder.ToTable("OrgUnitTypes", Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(x => x.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.LevelOrder)
                   .IsRequired();

            builder.Property(x => x.CanHaveChild)
                   .IsRequired();

            //  Indexes 
            builder.HasIndex(x => x.Code)
                   .IsUnique();

            builder.HasIndex(x => x.LevelOrder)
                   .IsUnique();
        }
    }
}
