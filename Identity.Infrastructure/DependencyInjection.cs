using Identity.Application.IServices;
using Identity.Domain;
using Identity.Domain.Constants;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();

            // 1. تسجيل DbContext
            var connectionString = configuration.GetConnectionString("DefaultConnection")
               ?? throw new ArgumentNullException(nameof(configuration));

            services.AddDbContext<IdentityDbContext>(options =>
                options.UseSqlServer(connectionString));

            // --- Configure Identity ---
            services.AddIdentity<AppUser, AppRole>(options =>
            {
                // إعدادات كلمة المرور
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                // إعدادات القفل بعد المحاولات الفاشلة
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
            })
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();

            // Permission policies
            services.AddAuthorization(options =>
            {
                foreach (var permission in Permissions.AllPermissions)
                {
                    options.AddPolicy($"Permission.{permission}", policy =>
                        policy.RequireClaim("Permission", permission));
                }
            });

            // 3. إعدادات الـ Cookie الخاص بـ MVC
            services.ConfigureApplicationCookie(options =>
            {
                // مسار صفحة تسجيل الدخول إذا حاول المستخدم الدخول لصفحة محمية (Authorize)
                options.LoginPath = "/Account/Login";

                // مسار صفحة "غير مصرح لك" إذا كان لديه حساب لكن ليس لديه الصلاحية (Role)
                options.AccessDeniedPath = "/Account/AccessDenied";

                options.Cookie.Name = "ErpGovernorateAuthCookie";
                options.Cookie.HttpOnly = true; // حماية من هجمات XSS
                options.ExpireTimeSpan = TimeSpan.FromDays(7); // مدة الجلسة
                options.SlidingExpiration = true; // تجديد الجلسة طالما المستخدم نشط
            });


            return services;
        }
    }
}