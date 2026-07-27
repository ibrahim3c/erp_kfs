using HR.Domain.Terminations.Enums;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Terminations
{
    public class TerminationDecision : Entity
    {
        public Guid EmployeeId { get; private set; }
        public string DecisionNumber { get; private set; }
        public TerminationReason Reason { get; private set; }
        public DateTime DecisionDate { get; private set; }
        public DateTime LastWorkingDay { get; private set; }
        public string? LegalBasis { get; private set; }
        public string? AttachmentPath { get; private set; }
        public TerminationStatus Status { get; private set; }
        public string? CancellationReason { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }


        private TerminationDecision() { } // EF

        private TerminationDecision(Guid id, Guid employeeId, string decisionNumber, TerminationReason reason,
            DateTime decisionDate, DateTime lastWorkingDay, string? legalBasis, string? attachmentPath) : base(id)
        {
            
            EmployeeId = employeeId;
            DecisionNumber = decisionNumber;
            Reason = reason;
            DecisionDate = decisionDate;
            LastWorkingDay = lastWorkingDay;
            LegalBasis = legalBasis;
            AttachmentPath = attachmentPath;
            Status = TerminationStatus.Executed;
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public static Result<TerminationDecision> Create(
            Guid employeeId, string decisionNumber, TerminationReason reason,
            DateTime decisionDate, DateTime lastWorkingDay, string? legalBasis, string? attachmentPath)
        {
            if (string.IsNullOrWhiteSpace(decisionNumber))
                return Result<TerminationDecision>.Failure(TerminationErrors.InvalidDecisionNumber);

            if (lastWorkingDay < decisionDate.AddYears(-1))
                return Result<TerminationDecision>.Failure(TerminationErrors.InvalidDates);

            return Result<TerminationDecision>.Success(new TerminationDecision(
                Guid.NewGuid(), employeeId, decisionNumber, reason, decisionDate, lastWorkingDay, legalBasis, attachmentPath));
        }

        public Result Cancel(string reason)
        {
            if (Status == TerminationStatus.Cancelled)
                return Result.Failure(TerminationErrors.AlreadyCancelled);

            if (string.IsNullOrWhiteSpace(reason))
                return Result.Failure(TerminationErrors.CancellationReasonRequired);

            Status = TerminationStatus.Cancelled;
            CancellationReason = reason;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
