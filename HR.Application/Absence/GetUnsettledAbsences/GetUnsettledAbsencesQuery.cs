using Modules.Shared.Application.Messaging;

namespace HR.Application.Absence.GetUnsettledAbsences
{
    public record GetUnsettledAbsencesQuery(
        int Month,
        int Year
    ) : IQuery<List<UnsettledAbsenceResponse>>;
}
