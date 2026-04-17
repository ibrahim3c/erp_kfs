using HR.Domain.Loans;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly HRDbContext dbContext;

        public LoanRepository(HRDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public void Add(Loan loan)
        {
            dbContext.Loans.Add(loan);
        }

        public async Task AddAsync(Loan loan, CancellationToken cancellationToken)
        {
            await dbContext.Loans.AddAsync(loan,cancellationToken);
        }

        public void Delete(Loan loan)
        {
            dbContext.Loans.Remove(loan);
        }

        public async Task<List<Loan>> GetActiveLoansByMonthAsync(int month, int year, CancellationToken cancellationToken)
        {
            return await dbContext.Loans
                       .Include(l => l.Installments)
                       .Where(l => !l.IsCompleted
                                && l.StartDate <= new DateTime(year, month, 1))
                       .ToListAsync(cancellationToken);
        }

        public async Task<List<Loan>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await dbContext.Loans.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<Loan>> GetAllWithEmployeeAsync(CancellationToken cancellationToken)
        {
           return await dbContext.Loans.Include(x => x.Employee).AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Loan> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await dbContext.Loans.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Loan> GetByIdWithEmployeeAsync(Guid id, CancellationToken cancellationToken)
        {
           return await dbContext.Loans.Include(x => x.Employee).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public void Update(Loan loan)
        {
            dbContext.Loans.Update(loan);
        }
    }
}
