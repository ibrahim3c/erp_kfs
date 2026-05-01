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
                "Server=db48963.public.databaseasp.net; Database=db48963; User Id=db48963; Password=3Db+Y#9r2Nk@; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;");

            return new GeographyDbContext(optionsBuilder.Options);
        }
    }
}
