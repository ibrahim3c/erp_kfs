using HR.Domain.Candidates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Infrastructure.Database;

namespace HR.Infrastructure.Persistance.Configurations.Candidates
{
    public class CandidateConfiguration : IEntityTypeConfiguration<Candidate>
    {
        public void Configure(EntityTypeBuilder<Candidate> builder)
        {
            builder.ToTable("Candidates",Schemas.HR); // تحديد اسم الجدول والـ Schema

            builder.HasKey(c => c.Id); // موروث من BaseEntity

            builder.Property(c => c.FullName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.NationalId)
                   .IsRequired()
                   .HasMaxLength(14); // الرقم القومي المصري 14 رقم

            // التأكد من عدم تكرار الرقم القومي
            builder.HasIndex(c => c.NationalId).IsUnique();

            builder.Property(c => c.Phone)
                   .HasMaxLength(20);

            builder.Property(c => c.Email)
                   .HasMaxLength(150);

            //// إعداد العلاقة مع ملفات الترشيح (One-to-Many)
            //// استخدام Metadata لتعريف EF Core بالمتغير المخفي _nominationFiles
            //builder.HasMany(c => c.NominationFiles)
            //       .WithOne() // لا يوجد Navigation Property للـ Candidate داخل NominationFile
            //       .HasForeignKey(nf => nf.CandidateId)
            //       .OnDelete(DeleteBehavior.Cascade); // عند حذف المرشح تُحذف ملفاته

            //builder.Metadata.FindNavigation(nameof(Candidate.NominationFiles))
            //       ?.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
