using HR.Domain.Secondments;
using HR.Domain.Secondments.Enums;
using HR.Infrastructure.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class SecondmentRepository : ISecondmentRepository
    {
        private readonly HRDbContext _dbContext;

        public SecondmentRepository(HRDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Secondment secondment)
        {
            _dbContext.Secondments.Add(secondment);
        }

        public async Task<bool> CheckItIsActive(Guid EmpId, SecondmentStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Secondments.AnyAsync(x => x.EmployeeId == EmpId && x.Status == status, cancellationToken);
        }

        public void Delete(Secondment secondment)
        {
            _dbContext.Secondments.Remove(secondment);
        }

        public async Task<Secondment?> GetByIdAsync(Guid secondmentId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Secondments.FindAsync(secondmentId, cancellationToken);
        }
    }
}
