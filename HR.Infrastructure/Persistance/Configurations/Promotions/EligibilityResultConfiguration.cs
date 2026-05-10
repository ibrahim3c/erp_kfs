using HR.Domain.Promotions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;


namespace HR.Infrastructure.Persistance.Configurations.Promotions
{
    public class EligibilityResultConfiguration : IEntityTypeConfiguration<EligibilityResult>
    {
        public void Configure(EntityTypeBuilder<EligibilityResult> builder)
        {
            builder.ToTable("EligibilityResults", Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ExclusionReason).IsRequired();
            builder.Property(x => x.EmployeeId).IsRequired();

            builder.Property(x => x.CurrentGradeLevel).IsRequired();
            builder.Property(x => x.CurrentGradeId).IsRequired();
            builder.Property(x => x.CurrentGradeCode).HasMaxLength(10).IsRequired();
            builder.Property(x => x.CurrentGradeName).HasMaxLength(100).IsRequired();
            builder.Property(x => x.ProposedGradeLevel);

            builder.Property(x => x.AvgKpiScore)
                   .HasPrecision(5, 2);

            builder.Property(x => x.YearsInCurrentGrade)
                   .HasPrecision(4, 1);

  

            // index لتسريع البحث
            builder.HasIndex(x => new { x.PromotionCycleId, x.EmployeeId })
                   .IsUnique();
            builder.HasIndex(x => x.Status);
        }
    }
}
