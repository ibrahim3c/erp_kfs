using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Dtos.LeadershipPosition
{
    public class AssignPositionDto
    {
        public Guid EmployeeId { get; set; }
        public Guid LeadershipPositionId { get; set; }
        public string? Notes { get; set; }
        public DateTime EndPositionForEmployee { get; set; }
    }
}
