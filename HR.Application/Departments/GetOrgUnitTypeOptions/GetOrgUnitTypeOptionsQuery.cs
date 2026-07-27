using Modules.Shared.Application.Messaging;

namespace HR.Application.Departments.GetOrgUnitTypeOptions
{
    public record GetOrgUnitTypeOptionsQuery() : IQuery<List<GetOrgUnitTypeOptionsResponse>>;
}
