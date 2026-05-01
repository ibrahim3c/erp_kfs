using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Organization.Infrastructure.Database
{
    public class OrganizationDbContextFactory
        : IDesignTimeDbContextFactory<OrganizationDbContext>
    {
        public OrganizationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrganizationDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=db48963.public.databaseasp.net; Database=db48963; User Id=db48963; Password=3Db+Y#9r2Nk@; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");

            return new OrganizationDbContext(optionsBuilder.Options);
        }
    }
}