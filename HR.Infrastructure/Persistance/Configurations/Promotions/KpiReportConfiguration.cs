using HR.Domain.Promotions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;


namespace HR.Infrastructure.Persistance.Configurations.Promotions
{
    public class KpiReportConfiguration : IEntityTypeConfiguration<KpiReport>
    {
        public void Configure(EntityTypeBuilder<KpiReport> builder)
        {
            builder.ToTable("KpiReports",Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.Year).IsRequired();
            builder.Property(x => x.Score)
                   .HasPrecision(5, 2)
                   .IsRequired();
            builder.Property(x => x.Grade)
                   .HasMaxLength(30)
                   .IsRequired();

            // موظف واحد → تقرير واحد لكل سنة
            builder.HasIndex(x => new { x.EmployeeId, x.Year })
                   .IsUnique();

            // الربط بجدول الموظفين الموجود
            builder.HasOne(x => x.Employee)
                   .WithMany()
                   .HasForeignKey(x => x.EmployeeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
