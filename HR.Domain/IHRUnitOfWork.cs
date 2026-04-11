using HR.Domain.Candidates;
using HR.Domain.Decisions;
using HR.Domain.Employees;
using HR.Domain.Incentives;
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


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
