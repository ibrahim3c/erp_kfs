using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Infrastructure.Presistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Shared.Infrastructure.Presistance.Configurations
{
    public class CityCenterConfiguration : IEntityTypeConfiguration<CityCenter>
    {
        public void Configure(EntityTypeBuilder<CityCenter> builder)
        {
            builder.ToTable("CityCenters", Schemas.Shared);
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.Type)
                .IsRequired();

            // relationships
            builder.HasMany(x=>x.LocalUnits)
                .WithOne(x => x.CityCenter)
                .HasForeignKey(x => x.CityCenterId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x=> x.Villages)
                .WithOne(x => x.CityCenter)
                .HasForeignKey(x => x.CityCenterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
