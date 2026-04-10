using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Modules.Shared.Domain.Common.Governorates;
using Modules.Shared.Infrastructure.Presistance.Database;
using System.Reflection.Emit;


namespace Modules.Shared.Infrastructure.Presistance.Configurations
{
    public class GovernrateConfiguration : IEntityTypeConfiguration<Governorate>
    {
        public void Configure(EntityTypeBuilder<Governorate> builder)
        {
            builder.ToTable("Governorates", Schemas.Shared);
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(g => g.Code)
                .IsRequired()
                .HasMaxLength(10);

            // Configure the relationship with CityCenters
            builder.HasMany(x => x.CityCenters)
                .WithOne()
                .HasForeignKey(x => x.GovernorateId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            builder.HasData(
                      Governorate.Seed("القاهرة", "02"),
                      Governorate.Seed( "الجيزة", "02"),
                      Governorate.Seed( "الإسكندرية", "03"),
                      Governorate.Seed("الدقهلية", "050"),
                      Governorate.Seed( "البحر الأحمر", "065"),
                      Governorate.Seed( "البحيرة", "045"),
                      Governorate.Seed("الفيوم", "084"),
                      Governorate.Seed( "الغربية", "040"),
                      Governorate.Seed( "الإسماعيلية", "064"),
                      Governorate.Seed( "المنوفية", "048"),
                      Governorate.Seed( "المنيا", "086"),
                      Governorate.Seed( "القليوبية", "013"),
                      Governorate.Seed( "الوادي الجديد", "092"),
                      Governorate.Seed( "السويس", "062"),
                      Governorate.Seed( "اسوان", "097"),
                      Governorate.Seed( "اسيوط", "088"),
                      Governorate.Seed( "بني سويف", "082"),
                      Governorate.Seed( "بورسعيد", "066"),
                      Governorate.Seed( "دمياط", "057"),
                      Governorate.Seed( "الشرقية", "055"),
                      Governorate.Seed( "جنوب سيناء", "069"),
                      Governorate.Seed( "كفر الشيخ", "047"),
                      Governorate.Seed( "مطروح", "046"),
                      Governorate.Seed( "الأقصر", "095"),
                      Governorate.Seed( "قنا", "096"),
                      Governorate.Seed( "شمال سيناء", "068"),
                      Governorate.Seed( "سوهاج", "093")
                  );
        }
            
    }
}
