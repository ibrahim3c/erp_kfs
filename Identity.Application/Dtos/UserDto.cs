using System;
namespace Identity.Application.Dtos
{
    public record UserDto(Guid Id, string Email, string UserName, IList<string> Roles);
}
