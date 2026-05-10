using Dapper;
using HR.Application.Employees.GetEmployeeDetails;
using HR.Application.Promotions.DTOs;
using HR.Domain;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Promotions.Entities;
using HR.Domain.Promotions.Enum;
using HR.Domain.Promotions.Interfaces;
using MediatR;
using Modules.Shared.Application.Database;
using Modules.Shared.Application.Messaging;
using Modules.Shared.Domain;


namespace HR.Application.Promotions.Commands
{
    public class ApprovePromotionHandler: ICommandHandler<ApprovePromotionCommand, ApprovePromotionResult>
    {
        private readonly IHRUnitOfWork uow;
        private readonly ISqlConnectionFactory sqlConnectionFactory;

        public ApprovePromotionHandler(IHRUnitOfWork _uow,ISqlConnectionFactory sqlConnectionFactory)
        {
     
            uow = _uow;
            this.sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Result<ApprovePromotionResult>> Handle(ApprovePromotionCommand request, CancellationToken ct)
        {
            // ── 1. جلب الكشف ──────────────────────────────────────────
            var cycle = await uow.PromotionCycleRepository.GetByIdAsync(request.CycleId, ct);
            if (cycle is null)
                return Result<ApprovePromotionResult>.Failure(new Error("CycleNotFound", "الكشف غير موجود"));

            // ── 2. تحديد المختارين ────────────────────────────────────
            foreach (var result in cycle.Results)
                result.SetSelected(
                    request.SelectedEmployeeIds.Contains(result.EmployeeId));

            // ── 3. اعتماد الكشف (Domain Rule) ─────────────────────────
            try { cycle.Approve(request.ApprovedByUserId); }
            catch (InvalidOperationException ex)
            { return Result<ApprovePromotionResult>.Failure(new Error("CycleNotApproved", ex.Message)); }

            // ── 4. جلب الـ DecisionType من الـ Lookups ────────────────
            // كل نوع حركة له قرار مناظر في جدول الإعدادات
            var decisionTypeId = await uow.DecisionRepository.GetIdByMovementTypeAsync(cycle.Type, ct);

            if (decisionTypeId == Guid.Empty)
                return Result<ApprovePromotionResult>.Failure(new Error("DecisionTypeNotFound", $"لم يتم تعريف نوع القرار لـ {cycle.Type} في الإعدادات"));

            // ── 5. معالجة كل موظف مختار ──────────────────────────────
            var selectedResults = cycle.Results.Where(r => r.IsSelected).ToList();
            var historyEntries = new List<PromotionHistory>();
            var decisionsByEmpId = new Dictionary<Guid, Guid>(); // empId → decisionId

            foreach (var result in selectedResults)
            {
                // 5a. تحديد الدرجة الجديدة
                Guid targetGradeId = result.CurrentGradeId;

                if (cycle.Type == CycleType.Promotion
                    && result.ProposedGradeLevel.HasValue)
                {
                    using var connection = sqlConnectionFactory.CreateConnection();
                    const string sql = "SELECT Id FROM Organization.JobGrades WHERE GradeLevel = @level";
                    
                    var proposed = await connection.QuerySingleOrDefaultAsync<GetJobGradResponse>(
                        sql, new { level = result.ProposedGradeLevel.Value });

                    if (proposed is null)
                        return Result<ApprovePromotionResult>.Failure(new Error("ProposedGradeNotFound", "الدرجة المقترحة غير موجودة"));

                    targetGradeId = proposed.Id;
                }

                // 5b. إنشاء EmployeeDecision (عن طريق Aggregate Root)
                var employee = await uow.EmployeeRepository.GetIncludeDecisionsAsync(result.EmployeeId, ct);

                if (employee is null) continue;

                string decisionDescription = BuildDecisionDescription(
                    cycle.Type, result, targetGradeId);

                var decisionResult = EmployeeDecision.Create(
                    employeeId: result.EmployeeId,
                    decisionId: decisionTypeId,
                    description: decisionDescription,
                    validFrom: cycle.EligibilityDate,
                    validTo: null,
                    status: EmployeeDecisionStatus.Active,
                    notes: $"صادر باعتماد كشف رقم {cycle.Id}");


                if (decisionResult.IsFailure)
                    return Result<ApprovePromotionResult>.Failure(new Error("DecisionCreationFailed", $"فشل إنشاء قرار للموظف: {decisionResult.Error.Name}"));

                // جيب الـ Decision اللي اتضافت للتو (آخر واحدة)
                var newDecision = decisionResult.Value;
                decisionsByEmpId[result.EmployeeId] = newDecision.Id;

                // 5c. تحديث درجة الموظف (للترقية فقط)
                if (cycle.Type == CycleType.Promotion)
                    employee.AssignToPosition(
                        employee.OrgUnitId,
                        employee.JobTitleId,
                        targetGradeId,
                        employee.FunctionalGroupId);

                // 5d. إنشاء PromotionHistory مرتبط بالقرار
                var history = PromotionHistory.Create(
                    employeeId: result.EmployeeId,
                    fromGradeId: result.CurrentGradeId,
                    toGradeId: targetGradeId,
                    effectiveDate: cycle.EligibilityDate,
                    movementType: cycle.Type,
                    cycleId: cycle.Id,
                    linkedDecisionId: newDecision.Id,  // ← الربط
                    notes: decisionDescription);

                historyEntries.Add(history.Value);
            }

            // ── 6. حفظ كل شيء في Transaction واحدة ──────────────────
                   
                 uow.PromotionCycleRepository.Update(cycle);
                await uow.PromotionHistoryRepository.AddRangeAsync(historyEntries, ct);
                await uow.SaveChangesAsync(ct);
     
            var promotionResult = new ApprovePromotionResult(
                Success: true,
                Message: $"تم الاعتماد — {selectedResults.Count} موظف — وإنشاء القرارات تلقائياً",
                ApprovedCount: selectedResults.Count);

            return Result<ApprovePromotionResult>.Success(promotionResult);
        }

        // ── Helpers ───────────────────────────────────────────────────

        private static string BuildDecisionDescription(
            CycleType type, EligibilityResult result, Guid targetGradeId)
            => type switch
            {
                CycleType.Promotion => $"ترقية من {result.CurrentGradeName} إلى الدرجة الأعلى",
                CycleType.Periodic => $"منح علاوة دورية سنوية — {result.CurrentGradeName}",
                CycleType.Incentive => $"منح علاوة تشجيعية (كفاءة) — {result.CurrentGradeName}",
                _ => "حركة وظيفية"
            };

        private static ApprovePromotionResult Fail(string msg)
            => new(false, msg, 0);
    }
}
