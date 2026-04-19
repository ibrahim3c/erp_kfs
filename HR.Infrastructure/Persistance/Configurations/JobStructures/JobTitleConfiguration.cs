using HR.Domain.JobStructures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.JobStructures
{
    public class JobTitleConfiguration : IEntityTypeConfiguration<JobTitle>
    {
        public void Configure(EntityTypeBuilder<JobTitle> builder)
        {
            builder.ToTable("JobTitles", Schemas.HR);

            builder.HasKey(j => j.Id);

            builder.Property(j => j.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(j => j.Description)
                .HasMaxLength(500);

            // Relationships
            builder.HasOne<FunctionalGroup>(jt=>jt.FunctionalGroup)
                .WithMany(j => j.JobTitles)
                .HasForeignKey(j => j.FunctionalGroupId)
                .OnDelete(DeleteBehavior.Restrict);

            // لا نحتاج لتعريف العلاقة هنا مرة أخرى لأننا عرفناها في FunctionalGroupConfiguration
            // EF Core ذكي بما يكفي لربطها من الجانب الآخر، ولكن يمكن إضافتها للتأكيد إن أردت.
        }
    }
}
