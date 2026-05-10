using HR.Domain.Promotions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Configurations.Promotions
{
    public class PromotionHistoryConfiguration : IEntityTypeConfiguration<PromotionHistory>
    {
        public void Configure(EntityTypeBuilder<PromotionHistory> builder)
        {
            builder.ToTable("PromotionHistory",Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.FromGradeId).IsRequired();
            builder.Property(x => x.ToGradeId).IsRequired();
            builder.Property(x => x.EffectiveDate).IsRequired();
            builder.Property(x => x.MovementType).IsRequired();
            builder.Property(x => x.PromotionCycleId).IsRequired();
            builder.Property(x => x.Notes).HasMaxLength(500);

            // الربط بكشف الترقية
            builder.HasOne(x => x.Cycle)
                   .WithMany()
                   .HasForeignKey(x => x.PromotionCycleId)
                   .OnDelete(DeleteBehavior.Restrict);

            // index لجلب آخر ترقية للموظف بسرعة
            builder.HasIndex(x => new { x.EmployeeId, x.EffectiveDate });
            builder.HasIndex(x => x.PromotionCycleId);
        }
    }
}
