using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.LeadershipPositionHistories
{
    public sealed class EmployeeLeadershipHistoryResponse
    {
        public Guid Id { get; init; }
        public string PositionName { get; init; }  // JobTitle - OrgUnit
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public string DecisionNumber { get; init; }
        public DateTime? DecisionDate { get; init; }
        public string Notes { get; init; }
    }
}
