using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Query.GetEmployeesForSelect
{
    public record EmployeeSelectDto(Guid Id, string Name, string JobTitle, string DepartmentName);
}
