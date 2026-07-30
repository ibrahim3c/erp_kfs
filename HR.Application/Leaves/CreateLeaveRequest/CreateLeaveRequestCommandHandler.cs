using HR.Domain;
using HR.Domain.Leaves;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;

namespace HR.Application.Leaves.CreateLeaveRequest
{
    public sealed class CreateLeaveRequestCommandHandler
        : ICommandHandler<CreateLeaveRequestCommand, Guid>
    {
        private readonly IHRUnitOfWork _unitOfWork;

        public CreateLeaveRequestCommandHandler(IHRUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateLeaveRequestCommand request,
            CancellationToken cancellationToken)
        {
            var durationDays = (request.EndDate - request.StartDate).Days + 1;

            if (request.LeaveCategory == LeaveCategory.Regular ||
                request.LeaveCategory == LeaveCategory.Casual)
            {
                var balance = await _unitOfWork.LeaveRepository
                    .GetBalanceAsync(request.EmployeeId, DateTime.Now.Year, cancellationToken);

                if (balance is null)
                {
                    balance = LeaveBalance.CreateDefault(request.EmployeeId, DateTime.Now.Year);
                    _unitOfWork.LeaveRepository.AddBalance(balance);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }

                if (request.LeaveCategory == LeaveCategory.Regular)
                {
                    var consumeResult = balance.ConsumeRegular(durationDays);
                    if (consumeResult.IsFailure)
                        return Result<Guid>.Failure(consumeResult.Error);
                }
                else
                {
                    var consumeResult = balance.ConsumeCasual(durationDays);
                    if (consumeResult.IsFailure)
                        return Result<Guid>.Failure(consumeResult.Error);
                }

                _unitOfWork.LeaveRepository.UpdateBalance(balance);
            }

            var leaveResult = LeaveRequest.Create(
                request.EmployeeId,
                request.LeaveCategory,
                request.StartDate,
                request.EndDate,
                request.ReplacementEmployeeId,
                request.ContactInfo,
                request.ReportAuthority,
                request.DecisionNumber,
                request.Diagnosis,
                request.ChildName,
                request.ChildDateOfBirth,
                request.AttachmentPath,
                request.Notes,
                request.PayPercentage);

            if (leaveResult.IsFailure)
                return Result<Guid>.Failure(leaveResult.Error);

            var leaveRequest = leaveResult.Value;
            _unitOfWork.LeaveRepository.AddRequest(leaveRequest);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(leaveRequest.Id);
        }
    }
}
