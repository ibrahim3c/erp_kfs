using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Identity.Domain;
using Identity.Application.IServices;
using ERP_KFS_MVC.ViewComponents;

namespace ERP_KFS_MVC.ViewComponents
{
    public class EmployeeSidebarViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public EmployeeSidebarViewComponent(
            UserManager<AppUser> userManager,
            IRoleService roleService,
            IUserService userService)
        {
            _userManager = userManager;
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(new EmployeeSidebarModel { FullName = "موظف" });

            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();

            List<string> permissions = new();
            foreach (var role in userRoles)
            {
                var rolePerms = await _roleService.GetRolePermissionsAsync(role);
                if (rolePerms.IsSuccess && rolePerms.Value != null)
                {
                    permissions.AddRange(rolePerms.Value);
                }
            }

            permissions = permissions.Distinct().ToList();

            var model = new EmployeeSidebarModel
            {
                FullName = user.UserName ?? "موظف",
                PermissionsList = permissions,
                UserRoles = userRoles
            };

            return View(model);
        }
    }
}