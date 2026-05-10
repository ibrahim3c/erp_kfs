using HR.Application.Promotions.DTOs;
using HR.Domain.Promotions.Enum;
using Modules.Shared.Application.Messaging;


namespace HR.Application.Promotions.Queries
{

    /// <summary>
    /// الـ Query اللي بيشغله HR لما يضغط "عرض كشف المستحقين"
    /// </summary>
    public record CheckEligibilityQuery(
     CycleType CycleType,
     DateTime EligibilityDate,
     int MinKpiScore,
     int MaxPenaltyDays,
     Guid RequestByUserId   
 ) : IQuery<CheckEligibilityResponse>;
}
