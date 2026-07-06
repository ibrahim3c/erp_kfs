using Dapper;
using Identity.Application.Dtos;
using Identity.Application.IServices;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Application.Database;
using Modules.Shared.Domain;

namespace Identity.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public AuthService(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            ITokenGenerator tokenGenerator,
            ISqlConnectionFactory sqlConnectionFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
            _sqlConnectionFactory = sqlConnectionFactory;
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


        // ----------------------JWT---------------------------
        public async Task<Result<AuthResponse>> LoginJwtAsync(LoginDto request)
        {
            // 1. البحث عن المستخدم بالإيميل أولاً
            var user = await _userManager.Users.Include(r => r.RefreshTokens).FirstOrDefaultAsync(x => x.Email == request.Email);
            if (user == null)
                return Result<AuthResponse>.Failure(IdentityErrors.InvalidCredentials);

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
                return Result<AuthResponse>.Failure(IdentityErrors.InvalidCredentials);

            var token = await _tokenGenerator.GenerateJwtTokenAsync(user);

            // get employee info from db using dapper
            using var connection = _sqlConnectionFactory.CreateConnection();

            const string sql = """
                    SELECT
                        e.Id,
                        e.Name,
                        e.Phone,
                        e.Email,
                        e.NationalId,
                        e.IsActive,
                        e.HireDate,
                        e.DateOfBirth,

                        jt.Name  AS JobTitleName


                    FROM HR.Employees e
                    LEFT JOIN Organization.JobTitles     jt  ON jt.Id  = e.JobTitleId
 
                    WHERE e.UserId = @EmployeeId
                    """;

            var response = await connection.QuerySingleOrDefaultAsync<EmployeeAuthResponse>(sql, new { EmployeeId = user.Id });

            var authResult = new AuthResponse
            {
                Token = token,
                EmployeeDetails = response
            };
            // check if user has already active refresh token 
            // so no need to give him new refresh token
            if (user.RefreshTokens.Any(r => r.IsActive))
            {
                // TODO: check this 
                var UserRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
                authResult.RefreshToken = UserRefreshToken.Token;
                authResult.RefreshTokenExpiresOn = UserRefreshToken.ExpiresOn;
            }

            // if he does not
            // generate new refreshToken
            else
            {
                var refreshToken = _tokenGenerator.GenereteRefreshToken();
                authResult.RefreshToken = refreshToken.Token;
                authResult.RefreshTokenExpiresOn = refreshToken.ExpiresOn;

                // then save it in db
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return Result<AuthResponse>.Success(authResult);
        }

        public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
        {
            // ensure there is user has this refresh token
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
            if (user == null)
                return Result<AuthResponse>.Failure(IdentityErrors.InvalidToken);

            // ensure this token is active
            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
                return Result<AuthResponse>.Failure(IdentityErrors.InvalidToken);

            // if all things well
            //revoke old refresh token
            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            // generate new refresh token and add it to db
            var newRefreshToken = _tokenGenerator.GenereteRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            // generate new JWT Token
            var jwtToken = await _tokenGenerator.GenerateJwtTokenAsync(user);

            return Result<AuthResponse>.Success(new AuthResponse
            {
                Token = jwtToken,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresOn = newRefreshToken.ExpiresOn
            });
        }

        public async Task<Result> RevokeTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
            if (user == null)
                return Result.Failure(IdentityErrors.InvalidToken);

            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
                return Result.Failure(IdentityErrors.InvalidToken);

            oldRefreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Result.Success();
        }
    }
}

