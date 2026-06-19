using Identity.Domain;


namespace Identity.Application.IServices
{
    public interface ITokenGenerator
    {
        Task<string> GenerateJwtTokenAsync(AppUser appUser);
        RefreshToken GenereteRefreshToken();
    }
}
