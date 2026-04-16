using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Loans
{
    public interface ILoanRepository
    {
        Task<Loan> GetByIdAsync(Guid id,CancellationToken cancellationToken);
        Task<List<Loan>> GetAllAsync(CancellationToken cancellationToken);

        // Eager loading of Employee details with the loan
        Task<List<Loan>> GetActiveLoansByMonthAsync(int month, int year, CancellationToken cancellationToken);
        Task<Loan> GetByIdWithEmployeeAsync(Guid id, CancellationToken cancellationToken);
        Task<List<Loan>> GetAllWithEmployeeAsync(CancellationToken cancellationToken);
        Task AddAsync(Loan loan,CancellationToken cancellationToken);
        void Add(Loan loan);
        void Update(Loan loan);
        void Delete(Loan loan);
    }
}
