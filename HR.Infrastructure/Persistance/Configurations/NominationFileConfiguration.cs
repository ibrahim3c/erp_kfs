using HR.Domain.Candidates;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace HR.Infrastructure.Persistance.Configurations
{
    public class NominationFileConfiguration : IEntityTypeConfiguration<NominationFile>
    {
        public void Configure(EntityTypeBuilder<NominationFile> builder)
        {
            builder.ToTable("NominationFiles", Schemas.HR);

            builder.HasKey(nf => nf.Id);

            builder.Property(nf => nf.FilePath)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(nf => nf.ReferenceNumber)
                   .HasMaxLength(100);

            // حفظ الـ Enum كرقم (Int) في الداتا بيز (وهو الافتراضي)، أو كنص (String) لو تفضل
            builder.Property(nf => nf.Status)
                   .IsRequired();
        }
    }
}
