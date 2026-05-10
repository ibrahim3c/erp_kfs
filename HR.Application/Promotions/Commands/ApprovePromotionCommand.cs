
using Modules.Shared.Application.Messaging;


namespace HR.Application.Promotions.Commands
{

    /// <summary>
    /// لما HR يضغط "اعتماد الكشف المختار"
    /// </summary>
    public record ApprovePromotionCommand(
        Guid CycleId,
        List<Guid> SelectedEmployeeIds,
        Guid ApprovedByUserId
    ) : ICommand<ApprovePromotionResult>;

    public record ApprovePromotionResult(
        bool Success,
        string Message,
        int ApprovedCount);
}
