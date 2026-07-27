using Modules.Shared.Application.Messaging;

namespace HR.Application.Absence.SettleAbsence
{
    public record SettleAbsenceCommand(
        Guid EmployeeId,
        string ActionType,
        string? Notes,
        int Month,
        int Year
    ) : ICommand<Guid>;
}
