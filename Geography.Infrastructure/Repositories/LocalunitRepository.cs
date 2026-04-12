using Geography.Infrastructure.Database;
using Modules.Shared.Infrastructure.Database;

namespace Geography.Domain.Repositories
{
    public class LocalunitRepository: BaseRepository<LocalUnit>,ILocalunitRepository
    {
        public LocalunitRepository(GeographyDbContext dbContext) : base(dbContext)
        {
        }
    }
}
