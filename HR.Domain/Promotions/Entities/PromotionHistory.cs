using HR.Domain.Employees;
using HR.Domain.Promotions.Enum;
using Modules.Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Promotions.Entities
{
    /// <summary>
    /// سجل تاريخ الترقيات — بيتبني تلقائياً لما نعتمد كشف
    /// هو المصدر الوحيد لـ GradeStartDate لكل موظف
    /// </summary>
    public class PromotionHistory : Entity
    {
        public Guid EmployeeId { get; private set; }
        public Guid FromGradeId { get; private set; }
        public Guid ToGradeId { get; private set; }
        public DateTime EffectiveDate { get; private set; }  // ← هو GradeStartDate
        public CycleType MovementType { get; private set; }  // Promotion/Periodic/Incentive
        public Guid PromotionCycleId { get; private set; }  // FK للكشف اللي اعتمده
        public Guid? LinkedDecisionId { get; private set; }  // FK لـ EmployeeDecision
        public string? Notes { get; private set; }

        // Navigations
        public PromotionCycle Cycle { get; private set; } = null!;

        private PromotionHistory(Guid id, Guid employeeId, Guid fromGradeId, Guid toGradeId, DateTime effectiveDate,
            CycleType movementType, Guid promotionCycleId, Guid? linkedDecisionId, string? notes) : base(id) { } // EF

        public static Result<PromotionHistory> Create(
            Guid employeeId,
            Guid fromGradeId,
            Guid toGradeId,
            DateTime effectiveDate,
            CycleType movementType,
            Guid cycleId,
            Guid? linkedDecisionId = null,
            string? notes = null)
        {
            if (employeeId == Guid.Empty)
                return Result<PromotionHistory>.Failure(PromotionErrors.EmployeeRequired);

            if (fromGradeId == toGradeId && movementType == CycleType.Promotion)
                return Result<PromotionHistory>.Failure(PromotionErrors.InvalidGradeChange);

            var history = new PromotionHistory(Guid.NewGuid(), employeeId, fromGradeId, toGradeId, effectiveDate,
                movementType, cycleId, linkedDecisionId, notes);

            return Result<PromotionHistory>.Success(history);
        }
    }
}

        