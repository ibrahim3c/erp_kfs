using Modules.Shared.Application.Messaging;

namespace HR.Application.Attendance.Commands.ConvertAbsenceToVacation
{
    public record ConvertAbsenceToVacationCommand(
        Guid AttendanceRecordId,
        string VacationType,
        string? Notes
    ) : ICommand<Guid>;
}
