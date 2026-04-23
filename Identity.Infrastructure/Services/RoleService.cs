using Identity.Application.Dtos;
using Identity.Application.IServices;
using Identity.Domain;
using Identity.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Domain;
using System.Security.Claims;

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

        // i got claims by role , and i got permissions by claims , so i can get permissions by role
        public async Task<Result<IEnumerable<string>>> GetRolePermissionsAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return Result<IEnumerable<string>>.Failure(IdentityErrors.RoleNotFound);

            var claims = await _roleManager.GetClaimsAsync(role);
            var permissions = claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            return Result<IEnumerable<string>>.Success(permissions);
        }

        public async Task<Result> AssignPermissionToRoleAsync(string roleName, string permission)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return Result.Failure(IdentityErrors.RoleNotFound);

            var claims = await _roleManager.GetClaimsAsync(role);
            var hasPermission = claims.Any(c => c.Type == "Permission" && c.Value == permission);
            if (hasPermission) return Result.Success();

            var result = await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));

            if (result.Succeeded) return Result.Success();

            return Result.Failure(new Error("Permission.CannotAssign", result.Errors.Select(e => e.Description).ToString()));
        }

        public async Task<Result> RemovePermissionFromRoleAsync(string roleName, string permission)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return Result.Failure(IdentityErrors.RoleNotFound);

            var claim = (await _roleManager.GetClaimsAsync(role))
                .FirstOrDefault(c => c.Type == "Permission" && c.Value == permission);

            if (claim == null) return Result.Success();

            var result = await _roleManager.RemoveClaimAsync(role, claim);

            if (result.Succeeded) return Result.Success();

            return Result.Failure(new Error("Permission.CannotRemove", result.Errors.Select(e => e.Description).ToString()));
        }

        public async Task<Result> AssignPermissionsToRoleAsync(string roleName, IEnumerable<string> permissions)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return Result.Failure(IdentityErrors.RoleNotFound);

            var existingClaims = await _roleManager.GetClaimsAsync(role);
            var existingPermissions = existingClaims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToHashSet();

            foreach (var permission in permissions)
            {
                if (!existingPermissions.Contains(permission))
                {
                    await _roleManager.AddClaimAsync(role, new Claim("Permission", permission));
                }
            }

            return Result.Success();
        }
    }
}
