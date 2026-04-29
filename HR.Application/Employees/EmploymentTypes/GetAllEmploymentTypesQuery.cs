using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Employees.EmploymentTypes
{
    public sealed record GetAllEmploymentTypesQuery() : IQuery<IEnumerable<EmploymentTypeDto>>;

    public sealed record EmploymentTypeDto(Guid Id, string Name, string Code);
}
