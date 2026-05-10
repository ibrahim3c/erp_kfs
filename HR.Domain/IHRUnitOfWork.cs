using HR.Domain.Attendance;
using HR.Domain.Candidates;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Incentives;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Permissions;
using HR.Domain.Promotions.Interfaces;
using HR.Domain.Terminations;

namespace HR.Domain
{
    public interface IHRUnitOfWork
    {
        ICandidateRepository CandidateRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDecisionRepository DecisionRepository { get; }
        IAcademicIncentiveRepository AcademicIncentiveRepository { get; }
        ITerminationRepository TerminationRepository { get; }
        ILoanRepository LoanRepository { get; }
        IPayrollRepository PayrollRepository { get; }
        IInsurancePurchaseRepository InsurancePurchaseRepository { get; }
        IPenaltyRepository PenaltyRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        ILateEntryRepository LateEntryRepository { get; }
        IAttendanceRecordRepository AttendanceRecordRepository { get; }
        IKpiReportRepository KpiReportRepository { get; }
        IPromotionCycleRepository PromotionCycleRepository { get; }
        IPromotionHistoryRepository PromotionHistoryRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
    }
}