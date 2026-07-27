using HR.Domain.ServiceTerms.Entities;
using HR.Infrastructure.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class ServiceTermRepository : IServiceTermRepository
    {
        private readonly HRDbContext _context;

        public ServiceTermRepository(HRDbContext context)
        {
            _context = context;
        }
        public void Add(ServiceTermRecord serviceTerm)
        {
            _context.ServiceTermRecords.Add(serviceTerm);
        }

        public void Delete(ServiceTermRecord serviceTerm)
        {
            _context.ServiceTermRecords.Remove(serviceTerm);
        }

        public async Task<ServiceTermRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.ServiceTermRecords.FindAsync(id, cancellationToken);
        }
    }
}
