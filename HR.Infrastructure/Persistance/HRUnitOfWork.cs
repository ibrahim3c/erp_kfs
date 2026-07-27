using HR.Domain;
using HR.Domain.Attendance;
using HR.Domain.Candidates;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Evaluations;
using HR.Domain.Funds;
using HR.Domain.Incentives;
using HR.Domain.Leaves;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Permissions;
using HR.Domain.Promotions.Interfaces;
using HR.Domain.Retirement.Entities;
using HR.Domain.Secondments;
using HR.Domain.Terminations;
using HR.Infrastructure.Persistance.Database;
using HR.Infrastructure.Persistance.Repositories;

namespace HR.Infrastructure.Persistance
{
    public class HRUnitOfWork : IHRUnitOfWork
    {
        private readonly HRDbContext _dbContext;

        public ICandidateRepository CandidateRepository { get; private set; }
        public IEmployeeRepository EmployeeRepository { get; private set; }
        public IDecisionRepository DecisionRepository { get; private set; }
        public IAcademicIncentiveRepository AcademicIncentiveRepository { get; private set; }
        public ITerminationRepository TerminationRepository { get; private set; }
        public ILoanRepository LoanRepository { get; private set; }
        public IPayrollRepository PayrollRepository { get; private set; }
        public IInsurancePurchaseRepository InsurancePurchaseRepository { get; private set; }
        public IPenaltyRepository PenaltyRepository { get; private set; }
        public IPermissionRepository PermissionRepository { get; private set; }
        public ILateEntryRepository LateEntryRepository { get; private set; }
        public IAttendanceRecordRepository AttendanceRecordRepository { get; private set; }

        public IKpiReportRepository KpiReportRepository { get; private set; }

        public IPromotionCycleRepository PromotionCycleRepository { get; private set; }

        public IPromotionHistoryRepository PromotionHistoryRepository { get; private set; }

        public IRetriementRepository RetriementRepository { get; private set; }

        public ISecondmentRepository SecondmentRepository { get; private set; }

        public IGrievanceRepository GrievanceRepository { get; private set; }

        public IFundRepository FundRepository { get; private set; }

        public ILeaveRepository LeaveRepository { get; private set; }

        public HRUnitOfWork(HRDbContext dbContext, ICandidateRepository candidateRepository)
        {
            _dbContext = dbContext;
            CandidateRepository = new CandidateRepository(_dbContext);
            EmployeeRepository = new EmployeeRepository(_dbContext);
            DecisionRepository = new DecisionRepository(_dbContext);
            AcademicIncentiveRepository = new IncentiveRepository(_dbContext);
            TerminationRepository = new TerminationRepository(_dbContext);
            LoanRepository = new LoanRepository(_dbContext);
            PayrollRepository = new PayrollRepository(_dbContext);
            InsurancePurchaseRepository = new InsurancePurchaseRepository(_dbContext);
            PenaltyRepository = new PenaltyRepository(_dbContext);
            PermissionRepository = new PermissionRepository(_dbContext);
            LateEntryRepository = new LateEntryRepository(_dbContext);
            AttendanceRecordRepository = new AttendanceRecordRepository(_dbContext);
            KpiReportRepository = new KpiReportRepository(_dbContext);
            PromotionCycleRepository = new PromotionCycleRepository(_dbContext);
            PromotionHistoryRepository = new PromotionHistoryRepository(_dbContext);
            RetriementRepository = new RetriementRepository(_dbContext);
            SecondmentRepository = new SecondmentRepository(_dbContext);
            GrievanceRepository = new GrievanceRepository(_dbContext);
            FundRepository = new FundRepository(_dbContext);
            LeaveRepository = new LeaveRepository(_dbContext);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

    }
}