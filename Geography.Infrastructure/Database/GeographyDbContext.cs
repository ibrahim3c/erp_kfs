using Geography.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Infrastructure.Database;
using System.Reflection;
namespace Geography.Infrastructure.Database
{
    public class GeographyDbContext : DbContext
    {
        public GeographyDbContext(DbContextOptions<GeographyDbContext> options) : base(options) { }

        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<CityCenter> CityCenters { get; set; }
        public DbSet<LocalUnit> LocalUnits { get; set; }
        public DbSet<Village> Villages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // تحديد الـ Schema الافتراضية
            modelBuilder.HasDefaultSchema(Schemas.Geopraphy);

            // هذا السطر يقوم بقراءة جميع كلاسات الـ Configuration اللي عملناها فوق وتطبيقها تلقائياً
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
