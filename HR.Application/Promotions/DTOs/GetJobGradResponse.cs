using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Promotions.DTOs
{
    public class GetJobGradResponse
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public int GradeLevel { get; private set; }
        public string Description { get; private set; }
        public int YearsNo { get; private set; }
        public bool IsActive { get; private set; }
    }
}
