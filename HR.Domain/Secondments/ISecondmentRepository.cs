using HR.Domain.Retirement.Entities;
using HR.Domain.Secondments.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HR.Domain.Secondments
{
    public interface ISecondmentRepository
    {
        void Add(Secondment secondment);
        void Delete(Secondment secondment);
        Task<Secondment?> GetByIdAsync(Guid secondmentId, CancellationToken cancellationToken = default);
        Task<bool> CheckItIsActive(Guid EmpId,SecondmentStatus status ,CancellationToken cancellationToken = default);
    }
}
