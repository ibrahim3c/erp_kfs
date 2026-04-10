using Microsoft.EntityFrameworkCore;
using Modules.Shared.Domain.Common.City_Center;
using Modules.Shared.Domain.Common.Governorates;
using System.Reflection;


namespace Modules.Shared.Infrastructure.Presistance.Database
{
    public abstract class SharedDbContext : DbContext
    {
        protected SharedDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<CityCenter> CityCenters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // حاجات مشتركة بين كل الـ modules

        }
    }
}
