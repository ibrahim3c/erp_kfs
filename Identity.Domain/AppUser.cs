using Microsoft.AspNetCore.Identity;

namespace Identity.Domain
{
    public class AppUser:IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}