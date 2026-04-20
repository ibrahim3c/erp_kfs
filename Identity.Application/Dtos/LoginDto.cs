namespace Identity.Application.Dtos
{
    public record LoginDto(string Email, string Password, bool RememberMe = false);
}
