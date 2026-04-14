using Modules.Shared.Application.Messaging;
namespace HR.Application.Employees.DeleteEmployee
{
    public sealed record DeleteEmployeeCommand(Guid Id) : ICommand;
}
