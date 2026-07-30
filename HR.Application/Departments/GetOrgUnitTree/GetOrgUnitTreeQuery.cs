using Modules.Shared.Application.Messaging;

namespace HR.Application.Departments.GetOrgUnitTree
{
    public record GetOrgUnitTreeQuery() : IQuery<List<GetOrgUnitTreeResponse>>;
}
