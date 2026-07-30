using HR.Domain.Transfers.Entities;
using HR.Infrastructure.Persistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Infrastructure.Persistance.Repositories
{
    public class TranseferRepository : ITranseferRepository
    {
        private readonly HRDbContext context;

        public TranseferRepository(HRDbContext _context)
        {
            context = _context;
        }
        public void AddExternalMovementAsync(ExternalMovement externalMovement)
        {
            context.ExternalMovements.Add(externalMovement);
        }

        public void AddInternalTransferAsync(InternalTransfer internalTransfer)
        {
            context.InternalTransfers.Add(internalTransfer);
        }
        

        public void DeleteExternalMovementAsync(ExternalMovement externalMovement)
        {
            context.ExternalMovements.Remove(externalMovement);
        }

        public void DeleteInternalTransferAsync(InternalTransfer internalTransfer)
        {
            context.InternalTransfers.Remove(internalTransfer);
        }

        public async Task<ExternalMovement?> GetExternalMovementByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.ExternalMovements.FindAsync(id, cancellationToken);
        }

        public async Task<InternalTransfer?> GetInternalTransferByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await context.InternalTransfers.FindAsync(id, cancellationToken);
        }
    }
}
