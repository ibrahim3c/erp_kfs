using Identity.Application.IServices;
using Identity.Domain;
using Identity.Domain.Constants;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Helpers;
using Identity.Infrastructure.Integration;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Modules.Shared.Application.Interfaces;
using System.Text;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IIdentityService, EmployeeIdentityService>();

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

            // -------------------JWT Authentication-------------------------


            // JWTHelper (1)
            services.Configure<JWT>(configuration.GetSection("JWT"));

            // (2)
            // to use jwt token to check authantication =>[authorize]
            services.AddAuthentication(options =>
            {
                // to change default authantication to jwt 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

                //  if u are unauthanticated it will redirect you to login form
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                // if there other schemas make is default of jwt
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


                // these configs to check if has token only but i want to check if he has right claims
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;

                // check if token have specific data
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidIssuer = configuration["JWT:Issuer"],
                    //ValidAudience = configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),

                    // if u want when the token expire he does not give me مهله بعض الوقت 
                    ClockSkew = TimeSpan.Zero

                };
            }

                         );


            return services;
        }
    }
}