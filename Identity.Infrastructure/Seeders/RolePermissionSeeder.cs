using Identity.Domain;
using Identity.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Identity.Infrastructure.Seeders
{
    public class RolePermissionSeeder
    {
        // one role can have multiple permissions, and one permission can be assigned to multiple roles (many-to-many),
        // and permissions and role are stored as claims in the database, so we need to seed the permissions for each role in the database
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            var rolePermissions = GetRolePermissionsMapping();

            foreach (var (roleName, permissions) in rolePermissions)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                var users = await userManager.GetUsersInRoleAsync(roleName);
                
                foreach (var user in users)
                {
                    var existingClaims = await userManager.GetClaimsAsync(user);
                    
                    foreach (var permission in permissions)
                    {
                        var hasPermission = existingClaims.Any(c => 
                            c.Type == "Permission" && c.Value == permission);
                        
                        if (!hasPermission)
                        {
                            await userManager.AddClaimAsync(user, new Claim("Permission", permission));
                        }
                    }
                }
            }
        }

        private static Dictionary<string, string[]> GetRolePermissionsMapping()
        {
            return new Dictionary<string, string[]>
            {
                [Roles.Admin] = Permissions.AllPermissions,

                [Roles.HR] = new[]
                {
                    Permissions.EmployeesView,
                    Permissions.EmployeesCreate,
                    Permissions.EmployeesEdit,
                    Permissions.EmployeesDelete,
                    Permissions.EmployeesSearch,
                    Permissions.PromotionsManage,
                    Permissions.TransfersManage,
                    Permissions.SecondmentsManage,
                    Permissions.TerminationsManage,
                    Permissions.RetirementManage,
                    Permissions.LeavesManage,
                    Permissions.AttendanceManage,
                    Permissions.AbsenceManage,
                    Permissions.LoansManage,
                    Permissions.PayrollView,
                    Permissions.FundsManage,
                    Permissions.EvaluationManage,
                    Permissions.DecisionsManage,
                    Permissions.PenaltiesManage,
                    Permissions.LegalReview,
                    Permissions.LookupsManage,
                    Permissions.DashboardView,
                },

                [Roles.Recruiter] = new[]
                {
                    Permissions.EmployeesView,
                    Permissions.EmployeesSearch,
                    Permissions.DashboardView,
                },

                [Roles.Employee] = new[]
                {
                    Permissions.EmployeesView,
                }
            };
        }
    }
}