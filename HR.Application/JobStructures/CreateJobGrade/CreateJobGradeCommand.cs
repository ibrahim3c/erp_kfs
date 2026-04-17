using Modules.Shared.Application.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.CreateJobGrade
{
    public record CreateJobGradeCommand(
        string Code,
        string Name,
        int GradeLevel,
        string Description,
        int YearsNo
    ) : ICommand<Guid>;
}
