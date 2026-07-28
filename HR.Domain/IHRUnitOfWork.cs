using HR.Domain.Attendance;
using HR.Domain.Candidates;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Evaluations;
using HR.Domain.Funds;
using HR.Domain.Incentives;
using HR.Domain.Leaves;
using HR.Domain.Legal;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Permissions;
using HR.Domain.Promotions.Interfaces;
using HR.Domain.Retirement.Entities;
using HR.Domain.Secondments;
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
        IRetriementRepository RetriementRepository { get; }
        ISecondmentRepository SecondmentRepository { get; }
        IGrievanceRepository GrievanceRepository { get; }
        IFundRepository FundRepository { get; }
        ILeaveRepository LeaveRepository { get; }
        ICourtRulingRepository CourtRulingRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        
    }
}