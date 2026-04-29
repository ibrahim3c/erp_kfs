using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Dtos;
using Identity.Application.IServices;
using ERP_KFS_MVC.Areas.Identity.ViewModels;

namespace ERP_KFS_MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        private static readonly string[] ProtectedRoles = { "Admin", "HR", "Employee" };

        public RolesController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _roleService.GetAllRolesAsync();
            if (!result.IsSuccess || result.Value == null)
                return View(new List<RoleDto>());

            var roles = result.Value.ToList();
            var stats = new Dictionary<Guid, RoleStatsViewModel>();

            foreach (var role in roles)
            {
                var usersResult = await _userService.GetAllUsersAsync();
                var userCount = 0;
                if (usersResult.IsSuccess && usersResult.Value != null)
                {
                    userCount = usersResult.Value.Count(u => 
                        u.Roles?.Contains(role.Name) == true);
                }

                var permsResult = await _roleService.GetRolePermissionsAsync(role.Name);
                var permCount = permsResult.Value?.Count() ?? 0;

                stats[role.Id] = new RoleStatsViewModel
                {
                    UserCount = userCount,
                    PermissionCount = permCount
                };
            }

            ViewBag.RoleStats = stats;
            ViewBag.ProtectedRoles = ProtectedRoles;
            return View(roles);
        }

        public IActionResult Create()
        {
            return View(new CreateRoleViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _roleService.CreateRoleAsync(model.Name);
            if (result.IsSuccess)
            {
                TempData["Success"] = $"تم إنشاء الدور '{model.Name}' بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error?.Name ?? "حدث خطأ");
            return View(model);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _roleService.GetRoleByIdAsync(id);
            if (result.IsSuccess && result.Value != null)
            {
                var isProtected = ProtectedRoles.Contains(result.Value.Name);
                ViewBag.IsProtected = isProtected;
                return View(new EditRoleViewModel
                {
                    Id = result.Value.Id,
                    Name = result.Value.Name
                });
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingResult = await _roleService.GetRoleByIdAsync(model.Id);
            if (existingResult.IsSuccess && existingResult.Value != null)
            {
                var isProtected = ProtectedRoles.Contains(existingResult.Value.Name);
                if (isProtected)
                {
                    TempData["Error"] = "لا يمكن تعديل الأدوار الأساسية للنظام.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var result = await _roleService.UpdateRoleAsync(model.Id, model.Name);
            if (result.IsSuccess)
            {
                TempData["Success"] = "تم تعديل الدور بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", result.Error?.Name ?? "حدث خطأ");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var roleResult = await _roleService.GetRoleByIdAsync(id);
            if (!roleResult.IsSuccess || roleResult.Value == null)
            {
                TempData["Error"] = "الدور غير موجود.";
                return RedirectToAction(nameof(Index));
            }

            var roleName = roleResult.Value.Name;
            var isProtected = ProtectedRoles.Contains(roleName);
            if (isProtected)
            {
                TempData["Error"] = "لا يمكن حذف الأدوار الأساسية للنظام.";
                return RedirectToAction(nameof(Index));
            }

            var usersResult = await _userService.GetAllUsersAsync();
            if (usersResult.IsSuccess && usersResult.Value != null)
            {
                var usersInRole = usersResult.Value.Count(u => u.Roles?.Contains(roleName) == true);
                if (usersInRole > 0)
                {
                    TempData["Error"] = $"لا يمكن حذف الدور '{roleName}' لأنه مُعيّن لـ {usersInRole} مستخدم.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var result = await _roleService.DeleteRoleAsync(id);
            if (result.IsSuccess)
            {
                TempData["Success"] = $"تم حذف الدور '{roleName}' بنجاح.";
            }
            else
            {
                TempData["Error"] = "حدث خطأ أثناء حذف الدور.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Users(Guid id)
        {
            var roleResult = await _roleService.GetRoleByIdAsync(id);
            if (!roleResult.IsSuccess || roleResult.Value == null)
                return NotFound();

            var usersResult = await _userService.GetAllUsersAsync();
            if (!usersResult.IsSuccess || usersResult.Value == null)
                return NotFound();

            var roleName = roleResult.Value.Name;
            var allUsers = usersResult.Value.ToList();

            var usersInRole = allUsers.Where(u => u.Roles?.Contains(roleName) == true).ToList();
            var usersNotInRole = allUsers.Where(u => u.Roles?.Contains(roleName) != true).ToList();

            var model = new RoleUsersViewModel
            {
                RoleId = roleResult.Value.Id,
                RoleName = roleName,
                UsersInRole = usersInRole.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName ?? u.Email,
                    Email = u.Email ?? ""
                }).ToList(),
                AllUsers = usersNotInRole.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName ?? u.Email,
                    Email = u.Email ?? ""
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserToRole(Guid roleId, Guid userId)
        {
            var roleResult = await _roleService.GetRoleByIdAsync(roleId);
            if (!roleResult.IsSuccess || roleResult.Value == null)
                return NotFound();

            var userResult = await _userService.GetUserByIdAsync(userId);
            if (!userResult.IsSuccess || userResult.Value == null)
                return NotFound();

            var manageResult = await _userService.ManageUserRolesAsync(new ManageRolesDto(
                userId,
                new List<RolesDto> { new RolesDto(roleResult.Value.Name, true) }
            ));

            if (manageResult.IsSuccess)
            {
                TempData["Success"] = $"تم إضافة '{userResult.Value.FullName}' للدور '{roleResult.Value.Name}'.";
            }
            else
            {
                TempData["Error"] = "حدث خطأ أثناء إضافة المستخدم للدور.";
            }

            return RedirectToAction(nameof(Users), new { id = roleId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveUserFromRole(Guid roleId, Guid userId)
        {
            var roleResult = await _roleService.GetRoleByIdAsync(roleId);
            if (!roleResult.IsSuccess || roleResult.Value == null)
                return NotFound();

            var userResult = await _userService.GetUserByIdAsync(userId);
            if (!userResult.IsSuccess || userResult.Value == null)
                return NotFound();

            if (roleResult.Value.Name == "Admin" && userResult.Value.Email == "admin@erp.com")
            {
                TempData["Error"] = "لا يمكن إزالة المدير العام من دور الأدمن.";
                return RedirectToAction(nameof(Users), new { id = roleId });
            }

            var currentRoles = userResult.Value.Roles?.ToList() ?? new List<string>();
            currentRoles.Remove(roleResult.Value.Name);

            var newRoles = currentRoles.Select(r => new RolesDto(r, true)).ToList();
            var manageResult = await _userService.ManageUserRolesAsync(new ManageRolesDto(userId, newRoles));

            if (manageResult.IsSuccess)
            {
                TempData["Success"] = $"تم إزالة '{userResult.Value.FullName}' من الدور '{roleResult.Value.Name}'.";
            }
            else
            {
                TempData["Error"] = "حدث خطأ أثناء إزالة المستخدم من الدور.";
            }

            return RedirectToAction(nameof(Users), new { id = roleId });
        }

        public async Task<IActionResult> Permissions(Guid id)
        {
            var roleResult = await _roleService.GetRoleByIdAsync(id);
            if (!roleResult.IsSuccess || roleResult.Value == null)
                return RedirectToAction(nameof(Index));

            var permsResult = await _roleService.GetRolePermissionsAsync(roleResult.Value.Name);
            var currentPermissions = permsResult.Value?.ToList() ?? new List<string>();

            var allPermissions = global::Identity.Domain.Constants.Permissions.AllPermissions
                .Select(p => new PermissionItem
                {
                    Id = p,
                    Name = p.Replace(".", " - "),
                    Category = p.Split('.')[0],
                    IsSelected = currentPermissions.Contains(p)
                })
                .ToList();

            var model = new AssignPermissionsViewModel
            {
                RoleId = roleResult.Value.Id,
                RoleName = roleResult.Value.Name,
                Permissions = allPermissions,
                SelectedPermissionIds = currentPermissions.ToArray()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignPermissions(AssignPermissionsViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var roleResult = await _roleService.GetRoleByIdAsync(model.RoleId);
            if (!roleResult.IsSuccess || roleResult.Value == null)
                return RedirectToAction(nameof(Index));

            var permissions = model.SelectedPermissionIds?.ToList() ?? new List<string>();
            await _roleService.AssignPermissionsToRoleAsync(roleResult.Value.Name, permissions);

            TempData["Success"] = $"تم تحديث صلاحيات الدور '{model.RoleName}' بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}