using Modules.Shared.Application.Messaging;
namespace HR.Application.Employees.GetEmployeeForEdit
{
    public sealed record GetEmployeeForEditQuery(Guid EmployeeId) : IQuery<GetEmployeeForEditResponse>;
}
