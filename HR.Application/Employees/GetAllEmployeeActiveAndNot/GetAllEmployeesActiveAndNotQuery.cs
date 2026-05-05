using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.GetAllEmployeeActiveAndNot
{
    public sealed record GetAllEmployeesActiveAndNotQuery() : IQuery<IEnumerable<GetAllEmployeesQueryActiveAndNotResponse>>;

}
