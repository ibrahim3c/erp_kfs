using Identity.Application.Dtos;
using Modules.Shared.Domain;
namespace Identity.Application.IServices
{
    public interface IUserService
    {
        Task<Result<List<UserDto>>> GetAllUsersAsync();

        Task<Result<UserDto>> GetUserByIdAsync(Guid userId);

        Task<Result<UserDto>> GetUserByEmailAsync(string email);

        Task<Result<Guid>> CreateUserAsync(CreateUserDto dto);

        Task<Result> UpdateUserAsync(UpdateUserDto dto);

        Task<Result> DeleteUserAsync(Guid userId);

        Task<Result> ChangePasswordAsync(ChangePasswordDto dto);

        Task<Result> LockUnlockAsync(Guid userId);

        Task<Result<List<string>>> GetUserRolesAsync(Guid userId);
        Task<Result> ManageUserRolesAsync(ManageRolesDto dto);
    }
}
