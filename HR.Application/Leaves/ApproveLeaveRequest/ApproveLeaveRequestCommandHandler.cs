using HR.Domain;
using HR.Domain.Leaves;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Leaves.ApproveLeaveRequest
{
    public sealed class ApproveLeaveRequestCommandHandler
        : ICommandHandler<ApproveLeaveRequestCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public ApproveLeaveRequestCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ApproveLeaveRequestCommand request,
            CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.LeaveRepository
                .GetRequestByIdAsync(request.LeaveRequestId, cancellationToken);

            if (leaveRequest is null)
                return Result.Failure(LeaveErrors.NotFound);

            var approveResult = leaveRequest.Approve();
            if (approveResult.IsFailure)
                return Result.Failure(approveResult.Error);

            _unitOfWork.LeaveRepository.UpdateRequest(leaveRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
