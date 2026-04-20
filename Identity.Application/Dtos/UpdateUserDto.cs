namespace Identity.Application.Dtos
{
    public record UpdateUserDto(Guid Id, string Email, string UserName);
}
