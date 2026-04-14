using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;
namespace HR.Application.Employees.GetEmployeeForDelete
{
    public sealed record GetEmployeeForDeleteQuery(Guid EmployeeId) : IQuery<GetEmployeeForDeleteResponse>;
}
