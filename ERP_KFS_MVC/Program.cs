using ERP_KFS_MVC.Extensions;
using Geography.Infrastructure;
using HR.Application;
using HR.Infrastructure;
using Identity.Infrastructure;
using Modules.Shared.Application;
using Modules.Shared.Infrastructure;
using Organization.Application;
using Organization.Infrastructure;

namespace ERP_KFS_MVC
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Services
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSharedInfrastructure(builder.Configuration);
            builder.Services.AddSharedApplication();
            builder.Services.AddHRInfrastructure(builder.Configuration);
            builder.Services.AddHRApplication();
            builder.Services.AddIdentityInfrastructure(builder.Configuration);
            builder.Services.AddGeographyInfrastructure(builder.Configuration);
            builder.Services.AddOrganizationInfrastructure(builder.Configuration);
            builder.Services.AddOrganizationApplication();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            var app = builder.Build();

            //  Apply Migrations + Seed
            await app.ApplyMigrationsAndSeedAsync();

            // Middleware
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ERP API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.MapControllerRoute(
               name: "areas",
               pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapControllers();
            app.Run();
        }
    }
}