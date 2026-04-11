using System.Data;

namespace Modules.Shared.Application.Database
{
    public interface ISqlConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
