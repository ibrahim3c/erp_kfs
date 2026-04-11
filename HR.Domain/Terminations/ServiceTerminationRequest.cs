using HR.Domain.Employees;
using Modules.Shared.Domain;

namespace HR.Domain.Terminations
{
    public sealed class ServiceTerminationRequest : Entity
    {
        private ServiceTerminationRequest() { }

        private ServiceTerminationRequest(
            Guid id,
            Guid employeeId,
            Guid serviceTerminationTypeId,
            string requestNumber,
            string issuedTo,
            DateTime requestDate,
            DateTime? requestStartDate,
            string reason,
            string filePath) : base(id)
        {
            EmployeeId = employeeId;
            ServiceTerminationTypeId = serviceTerminationTypeId;
            RequestNumber = requestNumber;
            IssuedTo = issuedTo;
            RequestDate = requestDate;
            RequestStartDate = requestStartDate;
            Reason = reason;
            FilePath = filePath;
            Status = TerminationRequestStatus.Pending;
        }

        public Guid EmployeeId { get; private set; }

        public Guid ServiceTerminationTypeId { get; private set; }

        public string RequestNumber { get; private set; }

        public string IssuedTo { get; private set; }

        public DateTime RequestDate { get; private set; }

        public DateTime? RequestStartDate { get; private set; }

        public string Reason { get; private set; }

        public TerminationRequestStatus Status { get; private set; }

        public string FilePath { get; private set; }

        public static Result<ServiceTerminationRequest> Create(
            Guid employeeId,
            Guid serviceTerminationTypeId,
            string requestNumber,
            string issuedTo,
            DateTime requestDate,
            DateTime? requestStartDate,
            string reason,
            string filePath)
        {
            if (employeeId == Guid.Empty)
                return Result<ServiceTerminationRequest>.Failure(EmployeeErrors.EmployeeIdEmpty);

            if (serviceTerminationTypeId == Guid.Empty)
                return Result<ServiceTerminationRequest>.Failure(EmployeeErrors.TerminationTypeIdEmpty);

            if (string.IsNullOrWhiteSpace(requestNumber))
                return Result<ServiceTerminationRequest>.Failure(EmployeeErrors.RequestNumberEmpty);

            if (requestStartDate.HasValue && requestStartDate < requestDate)
                return Result<ServiceTerminationRequest>.Failure(EmployeeErrors.InvalidRequestStartDate);

            var request = new ServiceTerminationRequest(
                Guid.NewGuid(),
                employeeId,
                serviceTerminationTypeId,
                requestNumber,
                issuedTo,
                requestDate,
                requestStartDate,
                reason,
                filePath
            );

            return Result<ServiceTerminationRequest>.Success(request);
        }

        // Business Behaviors
        public Result Approve()
        {
            if (Status == TerminationRequestStatus.Approved)
                return Result.Failure(EmployeeErrors.AlreadyApprovedTerminationRequest);

            Status = TerminationRequestStatus.Approved;

            return Result.Success();
        }

        public Result Reject(string reason)
        {
            Status = TerminationRequestStatus.Rejected;
            Reason = reason;

            return Result.Success();
        }

        public Result Cancel(string reason)
        {
            if (Status == TerminationRequestStatus.Cancelled)
                return Result.Failure(EmployeeErrors.AlreadyCancelledTerminationRequest);

            Status = TerminationRequestStatus.Cancelled;
            Reason = reason;

            return Result.Success();
        }
    }
}
