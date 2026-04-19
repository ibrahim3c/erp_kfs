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
    public class FunctionalGroupConfiguration : IEntityTypeConfiguration<FunctionalGroup>
    {
        public void Configure(EntityTypeBuilder<FunctionalGroup> builder)
        {
            builder.ToTable("FunctionalGroups", Schemas.HR);

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(f => f.Description)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne<QualitativeGroup>()
                .WithMany(f => f.FunctionalGroups)
                .HasForeignKey(f => f.QualitativeGroupId)
                .OnDelete(DeleteBehavior.Restrict); // منع الحذف إذا كان هناك مجموعات وظيفية مرتبطة

            // إخبار EF Core باستخدام الحقل الخاص (Private Field) لتعبئة قائمة المسميات
            //builder.Metadata.FindNavigation(nameof(FunctionalGroup.JobTitles))
            //    ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
