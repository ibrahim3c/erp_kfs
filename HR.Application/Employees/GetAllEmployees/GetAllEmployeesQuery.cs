using Modules.Shared.Application.Messaging;
namespace HR.Application.Employees.GetAllEmployees
{
    public sealed record GetAllEmployeesQuery() : IQuery<IEnumerable<EmployeeListResponse>>;
}
