using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Queries.GetAbsenceReport
{
    public record GetAbsenceReportQuery(
        DateTime DateFrom,
        DateTime DateTo,
        Guid? OrgUnitId
    ) : IQuery<AbsenceReportResponse>;
}
