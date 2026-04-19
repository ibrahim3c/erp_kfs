using erp_kfs.Host.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyERP.Web.Data;
using MyERP.Web.Models;

namespace erp_kfs.Host.Infrastructure.Seed
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedRolesAsync(roleManager);
            await SeedPermissionsAsync(context);
            await SeedAdminAsync(userManager, roleManager, context);
            await SeedRecruiterAsync(userManager, roleManager, context);
        }

        // ══════════════════════════════════════════════════════
        // 1. إنشاء الأدوار الأساسية
        // ══════════════════════════════════════════════════════
        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "HR", "Recruiter", "Employee" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                    Console.WriteLine($"✅ تم إنشاء الدور: {role}");
                }
            }
        }

        // ══════════════════════════════════════════════════════
        // 2. إنشاء الصلاحيات الأساسية (بدون تكرار)
        // ══════════════════════════════════════════════════════
        private static async Task SeedPermissionsAsync(ApplicationDbContext context)
        {
            var permissions = new[]
            {
                // التوظيف
                ("Employees.Create", "الموظفين - إضافة", "Recruitment"),
                ("Employees.View", "الموظفين - عرض", "Recruitment"),

                // لوحة التحكم
                ("Dashboard.View", "لوحة التحكم - عرض", "General"),

                // الموارد البشرية
                ("ServiceTerm.Manage", "ضم المدة الوظيفية - إدارة", "HR"),
                ("Decisions.Manage", "القرارات - إدارة", "HR"),
                ("Absence.Settlement", "الغياب - تسوية", "HR"),
                ("Promotions.Eligibility", "الترقيات - أهلية", "HR"),
                ("Transfers.Manage", "التحويلات - إدارة", "HR"),
                ("Secondments.Manage", "الانتدابات - إدارة", "HR"),

                // الحضور والانصراف
                ("Attendance.View", "الحضور والانصراف - عرض", "Attendance"),

                // الإجازات
                ("Leaves.Regular", "الإجازات - اعتيادية", "HR"),
                ("Leaves.Medical", "الإجازات - مرضية", "HR"),
                ("Leaves.Special", "الإجازات - خاصة", "HR"),
                ("Leaves.ManagerApprove", "الإجازات - موافقة المدير", "HR"),

                // التقييم
                ("Evaluation.AnnualReport", "التقييم - التقرير السنوي", "HR"),
                ("Evaluation.Grievances", "التقييم - الشكاوى", "HR"),

                // الجزاءات والقانونية
                ("Penalties.Manage", "الجزاءات - إدارة", "HR"),
                ("Legal.Rulings", "القانونية - الأحكام", "HR"),

                // المرتبات
                ("Payroll.Generate", "المرتبات - توليد", "Payroll"),
                ("Funds.Fellowship", "صناديق - الزمالة", "Payroll"),
                ("Loans.Requests", "القروض - الطلبات", "Payroll"),

                // المعاش وإنهاء الخدمة
                ("Retirement.Pending", "المعاش - قيد الانتظار", "HR"),
                ("Retirement.Files", "المعاش - الملفات", "HR"),
                ("Terminations.View", "إنهاء الخدمة - عرض", "HR"),
                ("Terminations.Manage", "إنهاء الخدمة - إدارة", "HR"),
                ("Terminations.Index", "إنهاء الخدمة - الفهرس", "HR"),

                // الإدارات والهيكل
                ("Departments.Manage", "الإدارات - إدارة", "Departments"),
                ("Lookups.Manage", "القيم المرجعية - إدارة", "General"),
                ("Leadership.Manage", "الهيكل القيادي - إدارة", "Leadership"),
                ("Permissions.View", "الصلاحيات - عرض", "General"),
            };

            bool anyAdded = false;
            foreach (var (name, displayName, category) in permissions)
            {
                if (!await context.Permissions.AnyAsync(p => p.Name == name))
                {
                    context.Permissions.Add(new Permission
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = name,
                        DisplayName = displayName,
                        Category = category
                    });
                    anyAdded = true;
                }
            }

            if (anyAdded)
            {
                await context.SaveChangesAsync();
                Console.WriteLine("✅ تم إنشاء الصلاحيات الأساسية.");
            }
        }

        // ══════════════════════════════════════════════════════
        // 3. إنشاء مستخدم Admin + كل الصلاحيات
        // ══════════════════════════════════════════════════════
        private static async Task SeedAdminAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            var adminEmail = "admin@erp.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            // إنشاء المستخدم لو مش موجود
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "المدير العام",
                    AnnualLeaveBalance = 0,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, "Admin@1234");
                if (!createResult.Succeeded)
                {
                    Console.WriteLine($"❌ فشل إنشاء Admin: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    return;
                }

                await userManager.AddToRoleAsync(adminUser, "Admin");
                Console.WriteLine("✅ تم إنشاء حساب Admin.");
            }

            // ✅ تحديث صلاحيات Admin دايماً (عشان لو اتضافت صلاحيات جديدة)
            var adminRole = await roleManager.FindByNameAsync("Admin");
            if (adminRole != null)
            {
                var allPermissions = await context.Permissions.ToListAsync();
                      var existingPermissionIds = (await context.RolePermissions
                .Where(rp => rp.RoleId == adminRole.Id)
                .Select(rp => rp.PermissionId)
                .ToListAsync())
                .ToHashSet();

                bool anyAdded = false;
                foreach (var perm in allPermissions)
                {
                    if (!existingPermissionIds.Contains(perm.Id))
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = adminRole.Id,
                            PermissionId = perm.Id
                        });
                        anyAdded = true;
                    }
                }

                if (anyAdded)
                {
                    await context.SaveChangesAsync();
                    Console.WriteLine("✅ تم تحديث صلاحيات Admin.");
                }
            }
        }

        // ══════════════════════════════════════════════════════
        // 4. إنشاء مستخدم Recruiter + صلاحية التوظيف فقط
        // ══════════════════════════════════════════════════════
        private static async Task SeedRecruiterAsync(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            var recruiterEmail = "recruiter@erp.com";
            var recruiterUser = await userManager.FindByEmailAsync(recruiterEmail);

            // إنشاء المستخدم لو مش موجود
            if (recruiterUser == null)
            {
                recruiterUser = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = recruiterEmail,
                    Email = recruiterEmail,
                    FullName = "مدير التوظيف",
                    AnnualLeaveBalance = 0,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(recruiterUser, "Recruiter@1234");
                if (!createResult.Succeeded)
                {
                    Console.WriteLine($"❌ فشل إنشاء Recruiter: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    return;
                }

                await userManager.AddToRoleAsync(recruiterUser, "Recruiter");
                Console.WriteLine("✅ تم إنشاء حساب Recruiter.");
            }

            // ✅ تعيين صلاحيات التوظيف فقط
            var recruiterRole = await roleManager.FindByNameAsync("Recruiter");
            if (recruiterRole != null)
            {
                var recruiterPermissionNames = new[]
                {
                    "Employees.Create",
                    "Employees.View"
                };

             var existingPermissionIds = (await context.RolePermissions
                    .Where(rp => rp.RoleId == recruiterRole.Id)
                    .Select(rp => rp.PermissionId)
                    .ToListAsync())
                    .ToHashSet();

                bool anyAdded = false;
                foreach (var permName in recruiterPermissionNames)
                {
                    var permission = await context.Permissions.FirstOrDefaultAsync(p => p.Name == permName);
                    if (permission != null && !existingPermissionIds.Contains(permission.Id))
                    {
                        context.RolePermissions.Add(new RolePermission
                        {
                            RoleId = recruiterRole.Id,
                            PermissionId = permission.Id
                        });
                        anyAdded = true;
                    }
                }

                if (anyAdded)
                {
                    await context.SaveChangesAsync();
                    Console.WriteLine("✅ تم تعيين صلاحيات Recruiter.");
                }
            }
        }
    }
}