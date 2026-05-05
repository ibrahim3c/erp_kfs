using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.GetAllEmployeeActiveAndNot
{
    public class GetAllEmployeesQueryActiveAndNotResponse
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public bool IsActive { get; init; }
        public string? JobTitleName { get; init; }
        public string? JobGradeName { get; init; }
        public string? OrgUnitName { get; init; }
        public Guid? LeadershipPositionId { get; init; }
    }
}
