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
                "Data Source=.;Initial Catalog=MyERPsystem;Integrated Security=True;Trust Server Certificate=True");

            return new OrganizationDbContext(optionsBuilder.Options);
        }
    }
}