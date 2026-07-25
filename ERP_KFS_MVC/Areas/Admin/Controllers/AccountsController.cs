using ERP_KFS_MVC.ViewModels;
using Identity.Application.Dtos;
using Identity.Application.IServices;
using Identity.Domain;
using Identity.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ERP_KFS_MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AllowAnonymous]
    public class AccountsController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAuthService _authService;

        public AccountsController(IAuthService authService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var loginResult = await _authService.LoginAsync(new LoginDto(
                    model.Email,
                    model.Password,
                    model.RememberMe
                ));

            if (loginResult.IsSuccess)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl)
                    && Url.IsLocalUrl(returnUrl)
                    && !returnUrl.Contains("/Admin/"))
                {
                    return LocalRedirect(returnUrl);
                }

                return RedirectToAction("Index", "Home", new { area = "" });
            }

            ModelState.AddModelError(string.Empty, loginResult.Error.Name);

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.RegisterAsync(new RegisterDto
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password
            });

            if (result.IsFailure)
            {
                ModelState.AddModelError(string.Empty, result.Error.Name);
                return View(model);
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {  
            await _authService.LogoutAsync();
            return RedirectToAction("Login", "Accounts", new { area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> LogoutGet()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login", "Accounts", new { area = "Admin" });
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}