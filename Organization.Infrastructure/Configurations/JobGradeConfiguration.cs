using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
using Organization.Domain;

namespace Organization.Infrastructure.Configurations
{
    public class JobGradeConfiguration : IEntityTypeConfiguration<JobGrade>
    {
        public void Configure(EntityTypeBuilder<JobGrade> builder)
        {
            builder.ToTable("JobGrades", Schemas.Organization);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.GradeLevel).IsRequired();
            builder.Property(x => x.YearsNo).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true);
        }
    }
}