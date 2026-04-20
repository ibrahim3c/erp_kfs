using Identity.Application.Dtos;
using Identity.Application.IServices;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Domain;

namespace Identity.Infrastructure.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<Result<IEnumerable<RoleDto>>> GetAllRolesAsync()
        {
            var roles = await _roleManager.Roles
                .Select(r => new RoleDto(r.Id, r.Name))
                .ToListAsync();

            return Result<IEnumerable<RoleDto>>.Success(roles);
        }

        public async Task<Result<RoleDto>> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return Result<RoleDto>.Failure(IdentityErrors.RoleNotFound);

            return Result<RoleDto>.Success(new RoleDto(role.Id, role.Name));
        }

        public async Task<Result<Guid>> CreateRoleAsync(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return Result<Guid>.Failure(IdentityErrors.RoleAlreadyExists);

            var role = new AppRole { Name = roleName };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded) return Result<Guid>.Success(role.Id);

            return Result<Guid>.Failure(new Error("Role.CannotCreated", result.Errors.Select(e => e.Description).ToArray().ToString()));
        }

        public async Task<Result> UpdateRoleAsync(Guid id, string newRoleName)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return Result.Failure(IdentityErrors.RoleNotFound);

            if (role.Name != newRoleName && await _roleManager.RoleExistsAsync(newRoleName))
                return Result.Failure(IdentityErrors.RoleAlreadyExists);

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded) return Result.Success();

            return Result.Failure( new Error("Role.CannotUpdated",result.Errors.Select(e => e.Description).ToArray().ToString()));
        }

        public async Task<Result> DeleteRoleAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) return Result.Failure(IdentityErrors.RoleNotFound);

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded) return Result.Success();

            return Result.Failure(new Error("Role.CannotDeleted", result.Errors.Select(e => e.Description).ToArray().ToString()));
        }
    }
}
