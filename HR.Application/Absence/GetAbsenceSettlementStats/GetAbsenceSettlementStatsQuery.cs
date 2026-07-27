using Modules.Shared.Application.Messaging;

namespace HR.Application.Absence.GetAbsenceSettlementStats
{
    public record GetAbsenceSettlementStatsQuery(
        int Month,
        int Year
    ) : IQuery<AbsenceSettlementStatsResponse>;
}
