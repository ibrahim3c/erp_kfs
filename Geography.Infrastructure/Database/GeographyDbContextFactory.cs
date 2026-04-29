using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geography.Infrastructure.Database
{
    public class GeographyDbContextFactory
        : IDesignTimeDbContextFactory<GeographyDbContext>
    {
        public GeographyDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GeographyDbContext>();

            optionsBuilder.UseSqlServer(
                "Data Source=.;Initial Catalog=MyERPsystem;Integrated Security=True;Trust Server Certificate=True");

            return new GeographyDbContext(optionsBuilder.Options);
        }
    }
}
