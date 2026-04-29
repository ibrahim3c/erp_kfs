using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Dtos;
using Identity.Application.IServices;
using ERP_KFS_MVC.Areas.Identity.ViewModels;

namespace ERP_KFS_MVC.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UsersController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _userService.GetAllUsersAsync();
            if (result.IsSuccess && result.Value != null)
                return View(result.Value);
            return View(new List<UserDto>());
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.IsSuccess && result.Value != null)
            {
                var rolesResult = await _roleService.GetAllRolesAsync();
                ViewBag.AllRoles = rolesResult.Value?.ToList() ?? new List<RoleDto>();
                return View(result.Value);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (result.IsSuccess && result.Value != null)
            {
                var rolesResult = await _userService.GetUserRolesAsync(id);
                var allRolesResult = await _roleService.GetAllRolesAsync();
                
                var model = new UserEditViewModel
                {
                    Id = result.Value.Id,
                    Email = result.Value.Email,
                    UserName = result.Value.FullName ,
                    CurrentRoles = rolesResult.Value?.ToList() ?? new List<string>(),
                    AllRoles = allRolesResult.Value?.ToList() ?? new List<RoleDto>()
                };
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UpdateUserDto(model.Id, model.Email, model.UserName);
            var result = await _userService.UpdateUserAsync(dto);

            if (result.IsSuccess)
            {
                var roleResult = await _userService.ManageUserRolesAsync(new ManageRolesDto(
                    model.Id,
                    model.SelectedRoles?.Select(r => new RolesDto(r, true)).ToList() ?? new List<RolesDto>()
                ));

                TempData["Success"] = "تم تحديث بيانات المستخدم بنجاح.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = result.Error?.Name ?? "حدث خطأ";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (result.IsSuccess)
            {
                TempData["Success"] = "تم حذف المستخدم بنجاح.";
            }
            else
            {
                TempData["Error"] = result.Error?.Name ?? "حدث خطأ أثناء الحذف.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUnlock(Guid id)
        {
            var result = await _userService.LockUnlockAsync(id);
            if (result.IsSuccess)
            {
                TempData["Success"] = "تم تحديث حالة账户 بنجاح.";
            }
            else
            {
                TempData["Error"] = result.Error?.Name ?? "حدث خطأ.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}