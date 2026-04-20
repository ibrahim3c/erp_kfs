
namespace Identity.Application.Dtos
{
    public record ManageRolesDto(Guid userId, List<RolesDto> Roles);

    public record RolesDto(string RoleName, bool IsSelected);
}
