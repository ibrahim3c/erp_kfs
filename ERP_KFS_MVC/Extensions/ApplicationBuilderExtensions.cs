using Identity.Infrastructure.Database;
using Identity.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

namespace ERP_KFS_MVC.Extensions
{

    public static class ApplicationBuilderExtensions
    {
        public static async Task ApplyMigrationsAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            // Apply migrations
            await dbContext.Database.MigrateAsync();

            // Seed data
            await IdentitySeeder.SeedAsync(scope.ServiceProvider);
        }
    }
}
