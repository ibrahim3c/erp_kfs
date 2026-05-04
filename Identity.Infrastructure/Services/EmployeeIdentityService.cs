using Identity.Application.IServices;
using Identity.Domain;
using Identity.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Modules.Shared.Application.Interfaces;
using Modules.Shared.Domain;

namespace Identity.Infrastructure.Services
{
    public class EmployeeIdentityService : IIdentityService
    {
        private readonly UserManager<AppUser> _userManager;
        private const string DefaultPassword = "P@ssw0rd123!";

        public EmployeeIdentityService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<Guid>> CreateUserForEmployeeAsync(
            string fullName,
            string nationalId,
            string? email)
        {
            var resolvedEmail = string.IsNullOrWhiteSpace(email)
                ? $"{nationalId}@erp.local"
                : email;

            var existingUser = await _userManager.FindByEmailAsync(resolvedEmail);
            if (existingUser != null)
                return Result<Guid>.Failure(IdentityErrors.EmailAlreadyRegistered);

            var user = new AppUser
            {
                UserName = fullName,
                Email = resolvedEmail,
                FullName = fullName,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, DefaultPassword);
            if (!result.Succeeded)
                return Result<Guid>.Failure(
                    new Error("User.EmployeeCreateFailed",
                        string.Join("; ", result.Errors.Select(e => e.Description))));

            await _userManager.AddToRoleAsync(user, Roles.Employee);

            return Result<Guid>.Success(user.Id);
        }

        public async Task<Result> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return Result.Failure(IdentityErrors.UserNotFound);

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
                return Result.Failure(IdentityErrors.UserCannotDelete);

            return Result.Success();
        }
    }
}
