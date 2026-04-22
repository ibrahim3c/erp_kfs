using Identity.Application.Dtos;
using Modules.Shared.Domain;

namespace Identity.Application.IServices
{
    public interface IRoleService
    {
        Task<Result<RoleDto>> GetRoleByIdAsync(Guid id);
        Task<Result<IEnumerable<RoleDto>>> GetAllRolesAsync();
        Task<Result<Guid>> CreateRoleAsync(string roleName);
        Task<Result> UpdateRoleAsync(Guid id, string newRoleName);
        Task<Result> DeleteRoleAsync(Guid id);

        Task<Result<IEnumerable<string>>> GetRolePermissionsAsync(string roleName);
        Task<Result> AssignPermissionToRoleAsync(string roleName, string permission);
        Task<Result> RemovePermissionFromRoleAsync(string roleName, string permission);
        Task<Result> AssignPermissionsToRoleAsync(string roleName, IEnumerable<string> permissions);
    }
}
