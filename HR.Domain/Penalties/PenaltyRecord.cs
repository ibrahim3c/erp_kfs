using HR.Domain.Employees;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Penalties
{
    public class PenaltyRecord : Entity
    {
        private PenaltyRecord() { }

        private PenaltyRecord(
            Guid id,
            Guid employeeId,
            DateTime violationDate,
            PenaltyActionType actionType,
            string penaltyType,
            decimal? deductionDays,
            DateTime executionMonth,
            string decisionReference,
            string notes,
            string attachmentPath) : base(id)
        {
            EmployeeId = employeeId;
            ViolationDate = violationDate;
            ActionType = actionType;
            PenaltyType = penaltyType;
            DeductionDays = deductionDays;
            ExecutionMonth = executionMonth;
            DecisionReference = decisionReference;
            Notes = notes;
            AttachmentPath = attachmentPath;
            CreatedAt = DateTime.UtcNow;
        }

        public Guid EmployeeId { get; private set; }
        public DateTime ViolationDate { get; private set; }
        public PenaltyActionType ActionType { get; private set; }
        public string PenaltyType {  get; private set; }
        public decimal? DeductionDays { get; private set; }
        public DateTime ExecutionMonth { get; private set; }
        public string DecisionReference { get; private set; }
        public string Notes { get; private set; }
        public string AttachmentPath { get; private set; }
        public DateTime CreatedAt { get; private set; }

        // Navigation Properties
        public Employee Employee { get; private set; }

        public static Result<PenaltyRecord> Create(
            Guid employeeId, DateTime violationDate,
            PenaltyActionType actionType,string penaltyType ,decimal? deductionDays,
            DateTime executionMonth, string decisionReference,
            string notes, string attachmentPath)
        {
            if (employeeId == Guid.Empty)
                return Result<PenaltyRecord>.Failure(PenaltyErrors.EmployeeEmpty);

            if ((actionType == PenaltyActionType.Deduct || actionType == PenaltyActionType.Hold) && (deductionDays == null || deductionDays <= 0))
                return Result<PenaltyRecord>.Failure(PenaltyErrors.InvalidDays);

            if (actionType == PenaltyActionType.Warning || actionType == PenaltyActionType.Postpone)
                deductionDays = 0;

            var penalty = new PenaltyRecord(
                Guid.NewGuid(), employeeId, violationDate,
                actionType,penaltyType, deductionDays, executionMonth, decisionReference, notes, attachmentPath);

            return Result<PenaltyRecord>.Success(penalty);
        }
    }
}
