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
    public class PromotionCycleConfiguration : IEntityTypeConfiguration<PromotionCycle>
    {
        public void Configure(EntityTypeBuilder<PromotionCycle> builder)
        {
            builder.ToTable("PromotionCycles",Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type).IsRequired();
            builder.Property(x => x.EligibilityDate).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.CreatedByUserId).IsRequired();
            builder.Property(x => x.IsApproved).HasDefaultValue(false);
            builder.Property(x => x.MinKpiScore).IsRequired();
            builder.Property(x => x.MaxPenaltyDays).IsRequired();


            builder.HasMany(x => x.Results)
                   .WithOne(x => x.Cycle)
                   .HasForeignKey(x => x.PromotionCycleId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
