using Modules.Shared.Application.Messaging;

namespace HR.Application.Employees.GetAllQualificationTypes
{
    public sealed record GetAllQualificationTypesQuery():IQuery<IEnumerable<GetAllQualificationTypesResponse>>;
}
