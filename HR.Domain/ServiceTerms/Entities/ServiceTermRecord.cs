using HR.Domain.ServiceTerms.Enums;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.ServiceTerms.Entities
{
    public class ServiceTermRecord : Entity
    {
        public Guid EmployeeId { get; private set; }
        public string PreviousEntityName { get; private set; }
        public ServiceType Type { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string? CommitteeDecisionNumber { get; private set; }
        public ServiceTermStatus Status { get; private set; }
        public DateTime? AdjustedSeniorityDate { get; private set; }
        public string? RejectionReason { get; private set; }

        public string? AttachmentPath { get; private set; }

        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        public TimeSpan NetDuration => EndDate - StartDate;

        private ServiceTermRecord() { } // EF

        private ServiceTermRecord(Guid id, Guid employeeId, string previousEntityName, ServiceType type,
            DateTime startDate, DateTime endDate, string? committeeDecisionNumber, string? attachmentPath) : base(id)
        {
           
            EmployeeId = employeeId;
            PreviousEntityName = previousEntityName;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            CommitteeDecisionNumber = committeeDecisionNumber;
            AttachmentPath = attachmentPath;
            Status = ServiceTermStatus.UnderReview;
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public static Result<ServiceTermRecord> Create(
            Guid employeeId, string previousEntityName, ServiceType type,
            DateTime startDate, DateTime endDate, string? committeeDecisionNumber, string? attachmentPath)
        {
            if (string.IsNullOrWhiteSpace(previousEntityName))
                return Result<ServiceTermRecord>.Failure(ServiceTermErrors.InvalidEntity);

            if (endDate <= startDate)
                return Result<ServiceTermRecord>.Failure(ServiceTermErrors.InvalidDates);

            return Result<ServiceTermRecord>.Success(new ServiceTermRecord(
                Guid.NewGuid(), employeeId, previousEntityName, type, startDate, endDate, committeeDecisionNumber, attachmentPath));
        }

        public Result Approve(DateTime employeeOriginalAppointmentDate)
        {
            if (Status != ServiceTermStatus.UnderReview)
                return Result.Failure(ServiceTermErrors.InvalidStatus);

            AdjustedSeniorityDate = employeeOriginalAppointmentDate - NetDuration;
            Status = ServiceTermStatus.Approved;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }

        public Result Reject(string reason)
        {
            if (Status != ServiceTermStatus.UnderReview)
                return Result.Failure(ServiceTermErrors.InvalidStatus);

            if (string.IsNullOrWhiteSpace(reason))
                return Result.Failure(ServiceTermErrors.ReasonRequired);

            Status = ServiceTermStatus.Rejected;
            RejectionReason = reason;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }
    }
}

