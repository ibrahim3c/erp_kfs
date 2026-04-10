using Modules.Shared.Domain.Common.Governorates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modules.Shared.Domain.Common.City_Center
{
    public interface ICityCenterRepository
    {
        Task<CityCenter> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<CityCenter>> GetAllAsync(CancellationToken cancellationToken = default);

        // (Eager Loading)


        void Add(CityCenter candidate);
        void Update(CityCenter candidate);
        void Delete(CityCenter candidate);
    }
}
