using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Secondments.Query.GetEmployeesForSelect
{
    public record GetEmployeesForSelectQuery(string? Search) : IQuery<List<EmployeeSelectDto>>;
}
