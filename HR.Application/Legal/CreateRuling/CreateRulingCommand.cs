using HR.Domain.Legal;
using Modules.Shared.Application.Messaging;

namespace HR.Application.Legal.CreateRuling
{
    public record CreateRulingCommand(
        string CaseNumber,
        string Year,
        Guid EmployeeId,
        string EmployeeName,
        string Summary,
        RulingExecutionType ExecutionType,
        string? AttachmentPath
    ) : ICommand<Guid>;
}
