using HR.Domain.Penalties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Application.Penalties.GetPenaltyList
{
    public class GetPenaltyListResponse
    {

        public Guid Id { get; init; }
        public string EmployeeName { get; init; } = string.Empty;
        public PenaltyActionType ActionType { get; init; } 
        public DateTime ViolationDate { get; init; }
        public string PenaltyType { get; init; } = string.Empty;
        public decimal? DeductionDays { get; init; }
        public DateTime ExecutionMonth { get; init; }
        public string DecisionReference { get; init; } = string.Empty;
        public string? AttachmentPathath { get; init; }
       
    }
}
