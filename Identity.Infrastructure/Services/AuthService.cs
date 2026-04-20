using Identity.Application.Dtos;
using Identity.Application.IServices;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Modules.Shared.Domain;

namespace Identity.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<Result<bool>> LoginAsync(LoginDto request)
        {
            // 1. البحث عن المستخدم بالإيميل أولاً
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                // أمنياً: نعطي رسالة موحدة حتى لا يعرف المخترق ما إذا كان الإيميل موجوداً أم لا
                return Result<bool>.Failure(IdentityErrors.InvalidCredentials);
            }

            // 2. محاولة تسجيل الدخول وإنشاء الـ Cookie
            // نستخدم user.UserName لأن SignInManager يحتاج الـ UserName الافتراضي 
            // (إلا إذا كنت قد جعلت الإيميل هو نفسه الـ UserName في نظامك)
            var result = await _signInManager.PasswordSignInAsync(
                user.UserName,
                request.Password,
                request.RememberMe, // إنشاء Cookie دائمة إذا اختار المستخدم "تذكرني"
                lockoutOnFailure: true // إغلاق الحساب مؤقتاً بعد عدة محاولات فاشلة
            );

            if (result.Succeeded)
            {
                return Result<bool>.Success(true);
            }

            // التعامل مع الحالات الخاصة (Best Practice)
            if (result.IsLockedOut)
            {
                return Result<bool>.Failure(IdentityErrors.AccountLockedOut);
            }

            if (result.IsNotAllowed)
            {
                return Result<bool>.Failure(IdentityErrors.NotAllowedToLogin);
            }

            return Result<bool>.Failure(IdentityErrors.InvalidCredentials);
        }

        public async Task<Result<bool>> LogoutAsync()
        {
            // هذه الدالة ستقوم بمسح الـ Cookie من المتصفح لإنهاء الجلسة
            await _signInManager.SignOutAsync();
            return Result<bool>.Success(true);
        }
    }
}
