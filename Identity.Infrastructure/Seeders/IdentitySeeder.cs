using Identity.Domain;
using Identity.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Identity.Infrastructure.Seeders
{
    public class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            foreach (var roleName in Roles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }

            await SeedAdminUserAsync(userManager);
        }

        private static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            var adminEmail = "admin@erp.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, Roles.Admin);
            }

            var claims = await userManager.GetClaimsAsync(adminUser);
            if (!claims.Any(c => c.Type == "Permission" && c.Value == Permissions.LeadershipManage))
            {
                await userManager.AddClaimAsync(adminUser, new Claim("Permission", Permissions.LeadershipManage));
            }
        }
    }
}