using HR.Domain;
using HR.Domain.Leaves;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Leaves.RejectLeaveRequest
{
    public sealed class RejectLeaveRequestCommandHandler
        : ICommandHandler<RejectLeaveRequestCommand>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public RejectLeaveRequestCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            RejectLeaveRequestCommand request,
            CancellationToken cancellationToken)
        {
            var leaveRequest = await _unitOfWork.LeaveRepository
                .GetRequestByIdAsync(request.LeaveRequestId, cancellationToken);

            if (leaveRequest is null)
                return Result.Failure(LeaveErrors.NotFound);

            var rejectResult = leaveRequest.Reject();
            if (rejectResult.IsFailure)
                return Result.Failure(rejectResult.Error);

            _unitOfWork.LeaveRepository.UpdateRequest(leaveRequest);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
