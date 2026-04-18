using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;

namespace erp_kfs.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));
            // --- Configure Identity ---
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = false; // (اختياري) تسهيل الباسورد أثناء التطوير
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

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


//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using MyERP.Web.Data;
//// 1. استدعاء مسارات الـ Dependency Injection الخاصة بالموديولات
//using HR.Infrastructure;
//using Modules.Shared.Infrastructure;
//using Modules.Shared.Application;
//using HR.Application;


//var builder = WebApplication.CreateBuilder(args);

//// =======================================================
//// 1. إعدادات الـ UI والـ Controllers
//// =======================================================
//builder.Services.AddControllersWithViews();

//// =======================================================
//// 2. إعدادات النظام الأساسي (Identity & Auth)
//// =======================================================
////builder.Services.AddDbContext<ApplicationDbContext>(options =>
////    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
////);

////builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
////{
////    options.SignIn.RequireConfirmedAccount = true;
////    options.Password.RequireDigit = false; // (اختياري) تسهيل الباسورد أثناء التطوير
////    options.Password.RequiredLength = 6;
////})
////.AddEntityFrameworkStores<ApplicationDbContext>()
////.AddDefaultTokenProviders();

//// =======================================================
//// 3. تسجيل الموديولات (Modules Registration)
//// =======================================================

////// أ) الموديول المشترك (Shared Module)
////builder.Services.AddSharedInfrastructure(builder.Configuration);
////builder.Services.AddApplicationLayer();

////// ب) موديول الموارد البشرية (HR Module)
////builder.Services.AddHRInfrastructure(builder.Configuration);
////builder.Services.AddHRApplication();

//// ج) هنا يمكنك إضافة الموديولات المستقبلية بسهولة


//// =======================================================
//// 4. بناء التطبيق (Build App)
//// =======================================================
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthentication(); // ⚠️ أضفت هذا السطر لأنه ضروري قبل الـ Authorization عند استخدام Identity
//app.UseAuthorization();

//// =======================================================
//// 5. إعدادات التوجيه (Routing)
//// =======================================================

//// مسار المناطق (Areas) للموديولات
//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//// المسار الافتراضي (الرئيسية)
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();