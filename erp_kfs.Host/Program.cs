using HR.Infrastructure;
using HR.Application;
using Modules.Shared.Infrastructure;
using Modules.Shared.Application;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using erp_kfs.Host.Models;

namespace erp_kfs.Host
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));
            // --- Configure Identity ---
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false; 
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // policy and permission handlers
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission.Leadership.Manage", policy =>
                    policy.RequireClaim("Permission", "Leadership.Manage"));
            });

            // add dependancies
            builder.Services.AddSharedInfrastructure(builder.Configuration);
            builder.Services.AddSharedApplication();
            builder.Services.AddHRInfrastructure(builder.Configuration);
            builder.Services.AddHRApplication(); 
  

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                await IdentitySeeder.SeedAsync(scope.ServiceProvider);
            }
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication(); 
            app.UseAuthorization();
            app.MapRazorPages();


            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            // ---------------------------------------------------------
            // 2. المسار الافتراضي (الصفحة الرئيسية للمشروع ككل)
            // ---------------------------------------------------------
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

