using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Transefers.Query.GetDepartmentsForSelect
{
    public record DepartmentSelectDto(Guid Id, string Name);
    public record JobTitleDto(Guid Id, string Name);
    public record DepartmentWithJobTitlesDto(List<DepartmentSelectDto> Departments, List<JobTitleDto> JobTitles);
}
