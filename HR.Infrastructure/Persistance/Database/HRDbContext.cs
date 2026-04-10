using HR.Domain.Candidates;
using HR.Domain.Employees;
using HR.Domain.Organization;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Infrastructure.Presistance.Database;
using System.Reflection;
namespace HR.Infrastructure.Persistance.Database
{
    public class HRDbContext : SharedDbContext
    {
        public HRDbContext(DbContextOptions<HRDbContext> options) : base(options)
        {
        }

        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }
        public DbSet<OrgUnitType> OrgUnitTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ← بييجي بـ Shared config

            // تحديد الـ Schema الافتراضية
            modelBuilder.HasDefaultSchema(Schemas.HR);

            // هذا السطر يقوم بقراءة جميع كلاسات الـ Configuration اللي عملناها فوق وتطبيقها تلقائياً
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
