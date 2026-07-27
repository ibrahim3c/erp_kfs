using Modules.Shared.Application.Messaging;

namespace HR.Application.Departments.GetDepartmentStats
{
    public record GetDepartmentStatsQuery() : IQuery<GetDepartmentStatsResponse>;
}
