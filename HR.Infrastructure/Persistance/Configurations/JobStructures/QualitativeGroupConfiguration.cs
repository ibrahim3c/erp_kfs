using HR.Domain.JobStructures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.JobStructures
{
    public class QualitativeGroupConfiguration : IEntityTypeConfiguration<QualitativeGroup>
    {
        public void Configure(EntityTypeBuilder<QualitativeGroup> builder)
        {
            builder.ToTable("QualitativeGroups", Schemas.HR);

            builder.HasKey(q => q.Id);

            builder.Property(q => q.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(q => q.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(q => q.Description)
                .HasMaxLength(500);


            // إخبار EF Core باستخدام الحقل الخاص (Private Field) لتعبئة القائمة
            //builder.Metadata.FindNavigation(nameof(QualitativeGroup.FunctionalGroups))
            //    ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}