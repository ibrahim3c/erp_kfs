using Modules.Shared.Domain;

namespace Modules.Shared.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<Guid>> CreateUserForEmployeeAsync(
            string fullName,
            string nationalId,
            string? email);

        Task<Result> DeleteUserAsync(Guid userId);
    }
}
