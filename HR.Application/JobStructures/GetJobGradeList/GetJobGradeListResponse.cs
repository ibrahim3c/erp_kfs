using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.JobStructures.GetJobGradeList
{
    public class GetJobGradeListResponse
    {
        public Guid Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public int GradeLevel { get; init; }
        public string Description { get; init; } = string.Empty;
        public int YearsNo { get; init; }
        public bool IsActive { get; init; }
    }
}
