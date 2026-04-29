using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Identity.Domain;
using Identity.Application.IServices;

namespace ERP_KFS_MVC.ViewComponents
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRoleService _roleService;

        public SidebarMenuViewComponent(UserManager<AppUser> userManager, IRoleService roleService)
        {
            _userManager = userManager;
            _roleService = roleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
                return View(new SidebarMenuViewModel());

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

            var model = new SidebarMenuViewModel
            {
                UserRoles = userRoles,
                Permissions = permissions.Distinct().ToList()
            };

            return View(model);
        }
    }
}