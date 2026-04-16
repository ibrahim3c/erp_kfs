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
    public class JobGradeConfiguration : IEntityTypeConfiguration<JobGrade>
    {
        public void Configure(EntityTypeBuilder<JobGrade> builder)
        {
            builder.ToTable("JobGrades", Schemas.HR);

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(g => g.Description)
                .HasMaxLength(500);

            builder.Property(g => g.GradeLevel)
                .IsRequired();

            builder.Property(g => g.YearsNo)
                .IsRequired();
        }
    }
}
