using HR.Domain.Retirement.Enums;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Retirement.Entities
{
    public class RetirementFile : Entity
    {
        public Guid EmployeeId { get; private set; }
        public DateTime ReferralDate { get; private set; }
        public RetirementReason Reason { get; private set; }
        public RetirementStage Stage { get; private set; }
        public Guid? ResponsibleEmployeeId { get; private set; }
        public string? Notes { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime UpdatedOn { get; private set; }

        // Checklist - الخطوة 1
        public bool JoinPeriodsAdded { get; private set; }
        public bool SpecialLeavesReviewed { get; private set; }

        private readonly List<RetirementSalaryRecord> _salaryRecords = new();
        public IReadOnlyCollection<RetirementSalaryRecord> SalaryRecords => _salaryRecords.AsReadOnly();

        private RetirementFile() { } // EF

        private RetirementFile(Guid id, Guid employeeId, DateTime referralDate, RetirementReason reason, Guid? responsibleEmployeeId)
            : base(id)
        {
            EmployeeId = employeeId;
            ReferralDate = referralDate;
            Reason = reason;
            Stage = RetirementStage.PendingReview;
            ResponsibleEmployeeId = responsibleEmployeeId;
            CreatedOn = DateTime.UtcNow;
            UpdatedOn = DateTime.UtcNow;
        }

        public static Result<RetirementFile> Create(Guid employeeId, DateTime referralDate, RetirementReason reason, Guid? responsibleEmployeeId)
        {
            if (employeeId == Guid.Empty)
                return Result<RetirementFile>.Failure(RetirementErrors.InvalidEmployee);

            if (!Enum.IsDefined(typeof(RetirementReason), reason))
                return Result<RetirementFile>.Failure(RetirementErrors.InvalidRetirementReason);

            return Result<RetirementFile>.Success(new RetirementFile(Guid.NewGuid(), employeeId, referralDate, reason, responsibleEmployeeId));
        }

        public Result UpdateChecklist(bool joinPeriodsAdded, bool specialLeavesReviewed)
        {
            JoinPeriodsAdded = joinPeriodsAdded;
            SpecialLeavesReviewed = specialLeavesReviewed;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }

        public Result AddOrUpdateSalaryYear(int year, decimal basicInsuredSalary)
        {
            var existing = _salaryRecords.FirstOrDefault(x => x.Year == year);
            if (existing is not null)
                existing.UpdateAmount(basicInsuredSalary);
            else
                _salaryRecords.Add(RetirementSalaryRecord.Create(year, basicInsuredSalary));

            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }

        // قاعدة عمل حقيقية بدل ما تبقى الحالة مجرد Enum بيتغير من برة
        public Result AdvanceStage(RetirementStage nextStage)
        {
            if (nextStage == RetirementStage.DeliveredToAuthority && (!JoinPeriodsAdded || !SpecialLeavesReviewed))
                return Result.Failure(RetirementErrors.ChecklistIncomplete);
             
            Stage = nextStage;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }

        public Result UpdateNotes(string? notes)
        {
            Notes = notes;
            UpdatedOn = DateTime.UtcNow;
            return Result.Success();
        }
    }
}
