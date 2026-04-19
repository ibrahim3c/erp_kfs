using HR.Domain.Employees;
using HR.Domain.Payrolls;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;
namespace HR.Infrastructure.Persistance.Configurations.Employees
{
    public class EmploymentTypeConfiguration : IEntityTypeConfiguration<EmploymentType>
    {
        public void Configure(EntityTypeBuilder<EmploymentType> builder)
        {
            builder.ToTable("EmploymentTypes",Schemas.HR);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.IsActive)
                .IsRequired();

            // Optional Index
            builder.HasIndex(x => x.Code)
                .IsUnique();

            //// إخبار EF Core باستخدام الحقل الخاص (Private Field) لتعبئة القائمة
            //builder.Metadata.FindNavigation(nameof(EmploymentType.PayrollCycles))
            //    ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }

}
