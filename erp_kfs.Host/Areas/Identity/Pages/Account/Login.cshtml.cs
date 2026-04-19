// Areas/Identity/Pages/Account/Login.cshtml.cs
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using erp_kfs.Host.Models;

namespace erp_kfs.Host.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager; // ← أضف هذا

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager) // ← أضف هذا في الـ constructor
        {
            _signInManager = signInManager;
            _userManager = userManager; // ← وأسند القيمة هنا
        }

        [BindProperty]
        public InputModel Input { get; set; } = new();

        public string? ReturnUrl { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
            [EmailAddress(ErrorMessage = "صيغة البريد غير صحيحة")]
            [Display(Name = "البريد الإلكتروني")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "كلمة المرور مطلوبة")]
            [DataType(DataType.Password)]
            [Display(Name = "كلمة المرور")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "تذكرني؟")]
            public bool RememberMe { get; set; }
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
        var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            // 👇 الحل: جيب المستخدم من الإيميل مباشرةً
            var user = await _userManager.FindByEmailAsync(Input.Email);

            if (user == null)
                return RedirectToPage("./Login");

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return LocalRedirect("~/Admin/Dashboard/Index");
            }
            else if (await _userManager.IsInRoleAsync(user, "HR"))
            {
                return LocalRedirect("~/HR/Home/Index");
            }
            else if (await _userManager.IsInRoleAsync(user, "Recruiter"))
            {
                return LocalRedirect("~/Employees/Create");
            }
            else
            {
                return LocalRedirect("~/Employees/myprofile");
            }
        }

        ModelState.AddModelError(string.Empty, "فشل محاولة تسجيل الدخول.");
    }

    return Page();
} 
       }
}