using Identity.Application.Dtos;
using Modules.Shared.Domain;
namespace Identity.Application.IServices
{
    public interface IAuthService
    {
        Task<Result<bool>> LoginAsync(LoginDto request);
        Task<Result<bool>> RegisterAsync(RegisterDto request);
        Task<Result<AuthResponse>> LoginJwtAsync(LoginDto request);
        Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
        Task<Result> RevokeTokenAsync(string refreshToken);
        Task<Result<bool>> LogoutAsync();
    }
}
