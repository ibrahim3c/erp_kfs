using Identity.Application.Dtos;
using Modules.Shared.Domain;
namespace Identity.Application.IServices
{
    public interface IAuthService
    {
        Task<Result<bool>> LoginAsync(LoginDto request);
        Task<Result<bool>> LogoutAsync();
    }
}
