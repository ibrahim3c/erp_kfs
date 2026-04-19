using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using erp_kfs.Host.Models;

namespace erp_kfs.Host.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "الاسم الكامل مطلوب")]
            [Display(Name = "الاسم الكامل")]
            public string FullName { get; set; } = string.Empty;

            [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
            [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
            [Display(Name = "البريد الإلكتروني")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "كلمة المرور مطلوبة")]
            [StringLength(100, ErrorMessage = "يجب أن تكون كلمة المرور بين {2} و{1} حرفًا.", MinimumLength = 4)]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            [Display(Name = "تأكيد كلمة المرور")]
            [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقتين.")]
            public string ConfirmPassword { get; set; } = string.Empty;
        }

        public void OnGet(string? returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    FullName = Input.FullName,
                    AnnualLeaveBalance = 7 // الرصيد الافتراضي للإجازات
                };

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("تم إنشاء مستخدم جديد بحساب.");

                    // === السطر اللي طلبتَه ===
                    await _userManager.AddToRoleAsync(user, "Employee");
                    // =========================

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // إذا فشل التحقق، أعد عرض النموذج مع الأخطاء
            return Page();
        }
    }
}