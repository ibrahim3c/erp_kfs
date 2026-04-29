using Identity.Application.Dtos;
using Identity.Application.IServices;
using Identity.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modules.Shared.Domain;
namespace Identity.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService( UserManager<AppUser> userManager )
        {
            _userManager = userManager;
        }

        public async Task<Result<List<UserDto>>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDtos.Add(new UserDto(
                    user.Id,
                    user.Email,
                    user.FullName,
                    roles));
            }

            return Result<List<UserDto>>.Success(userDtos);
        }

        public async Task<Result<UserDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return Result<UserDto>.Failure(IdentityErrors.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto(
                user.Id,
                user.Email,
                user.FullName,
                roles);

            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<UserDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return Result<UserDto>.Failure(IdentityErrors.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto(
                user.Id,
                user.Email,
                user.FullName,
                roles);

            return Result<UserDto>.Success(userDto);
        }

        public async Task<Result<Guid>> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
                return Result<Guid>.Failure(IdentityErrors.EmailAlreadyRegistered);

            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                EmailConfirmed = true,
                
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return Result<Guid>.Failure(
                    new Error("User.CreateFailed", result.Errors.Select(e => e.Description).ToString())
                );

            if (dto.Roles?.Any() == true)
                await _userManager.AddToRolesAsync(user, dto.Roles);

            return Result<Guid>.Success(user.Id);
        }

        public async Task<Result> UpdateUserAsync(UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());

            if (user == null)
                return Result.Failure(IdentityErrors.UserNotFound);

            user.Email = dto.Email;
            user.UserName = dto.UserName;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return Result.Failure(IdentityErrors.UserCannotUpdate);

            return Result.Success();
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

        public async Task<Result> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());

            if (user == null)
                return Result.Failure(IdentityErrors.UserNotFound);

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword);

            if (!result.Succeeded)
                return Result.Failure(IdentityErrors.PasswordChangeFailed);

            return Result.Success();
        }

        public async Task<Result> LockUnlockAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return Result.Failure(IdentityErrors.UserNotFound);

            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.UtcNow)
                user.LockoutEnd = DateTime.UtcNow.AddYears(1);
            else
                user.LockoutEnd = null;

            await _userManager.UpdateAsync(user);

            return Result.Success();
        }

        public async Task<Result<List<string>>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return Result<List<string>>.Failure(IdentityErrors.UserNotFound);

            var roles = await _userManager.GetRolesAsync(user);

            return Result<List<string>>.Success(roles.ToList());
        }
        public async Task<Result> ManageUserRolesAsync(ManageRolesDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.userId.ToString());

            if (user == null)
                return Result.Failure(IdentityErrors.UserNotFound);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in dto.Roles)
            {
                if (userRoles.Contains(role.RoleName) && !role.IsSelected)
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Contains(role.RoleName) && role.IsSelected)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
            }

            return Result.Success();
        }
    }
}
