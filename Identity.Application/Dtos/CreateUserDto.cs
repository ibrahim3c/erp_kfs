namespace Identity.Application.Dtos
{
    public record CreateUserDto( string Email, string UserName, string Password, List<string>? Roles = null);
}
