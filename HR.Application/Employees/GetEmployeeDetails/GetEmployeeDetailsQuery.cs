using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.GetEmployeeDetails
{
    public sealed record GetEmployeeDetailsQuery(Guid EmployeeId) : IQuery<GetEmployeeDetailsResponse>;
}
