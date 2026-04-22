using Identity.Infrastructure.Seeders;

namespace erp_kfs.Host.Extensions
{
    public static class IdentitySeederExtensions
    {
        public static async Task SeedIdentityAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            await Identity.Infrastructure.Seeders.IdentitySeeder.SeedAsync(scope.ServiceProvider);
            await RoleSeeder.SeedAsync(scope.ServiceProvider);
            await RolePermissionSeeder.SeedAsync(scope.ServiceProvider);
        }
    }
}