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
    }
}
