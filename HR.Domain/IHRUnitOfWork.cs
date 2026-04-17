using HR.Domain.Candidates;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Incentives;
using HR.Domain.JobStructures;
using HR.Domain.Loans;
using HR.Domain.Payrolls;
using HR.Domain.Penalties;
using HR.Domain.Terminations;
namespace HR.Domain
{
    public interface IHRUnitOfWork
    {
        //IStudentRepository StudentRepository { get; }

        ICandidateRepository CandidateRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IDecisionRepository DecisionRepository { get; }
        IAcademicIncentiveRepository AcademicIncentiveRepository { get; }
        ITerminationRepository TerminationRepository { get; }
        IJobStructureRepository JobStructureRepository { get; }
        ILoanRepository LoanRepository { get; }
        IPayrollRepository PayrollRepository { get; }
        IInsurancePurchaseRepository InsurancePurchaseRepository { get; }
        IPenaltyRepository PenaltyRepository { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
