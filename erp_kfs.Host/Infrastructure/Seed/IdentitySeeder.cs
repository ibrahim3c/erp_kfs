using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using erp_kfs.Host.Models;
using MyERP.Web.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Roles
        var roles = new[] { "Admin", "HR", "Employee" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Admin
        var adminEmail = "admin@erp.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "المدير العام",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, "Admin@123");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // HR
        var hrEmail = "hr@erp.com";
        var hrUser = await userManager.FindByEmailAsync(hrEmail);
        if (hrUser == null)
        {
            hrUser = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = hrEmail,
                Email = hrEmail,
                FullName = "مدير الموارد البشرية",
                AnnualLeaveBalance = 0,
                EmailConfirmed = true
            };
            var createResult = await userManager.CreateAsync(hrUser, "Hr@1234");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(hrUser, "HR");
                Console.WriteLine("✅ تم إنشاء حساب مدير الموارد البشرية (hr@erp.com) وكلمة المرور: Hr@1234");
            }
        }
    }
}