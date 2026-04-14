using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.UpdateEmployee
{
    public sealed record UpdateEmployeeCommand(
            Guid Id,
            string Name,
            string Code,
            string Email,
            string Phone,
            DateTime HireDate,
            bool IsActive,
            DateTime CreatedAt // Mapped from the hidden field
        ) : ICommand;
}
