using Organization.Domain;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Infrastructure.Database;
using System.Reflection;

namespace Organization.Infrastructure.Database
{
    public class OrganizationDbContext : DbContext
    {
        public OrganizationDbContext(DbContextOptions<OrganizationDbContext> options) : base(options) { }

        public DbSet<OrgUnitType> OrgUnitTypes { get; set; }
        public DbSet<OrgUnit> OrgUnits { get; set; }
        public DbSet<LeadershipPosition> LeadershipPositions { get; set; }
        public DbSet<LeadershipPositionHistory> LeadershipPositionHistories { get; set; }
        public DbSet<QualitativeGroup> QualitativeGroups { get; set; }
        public DbSet<FunctionalGroup> FunctionalGroups { get; set; }
        public DbSet<JobTitle> JobTitles { get; set; }
        public DbSet<JobGrade> JobGrades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schemas.Organization);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}