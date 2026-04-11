using Modules.Shared.Application.Database;
using Npgsql;
using System.Data;

namespace Modules.Shared.Infrastructure.Database
{
    internal sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionFactory;

        public SqlConnectionFactory(string connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(_connectionFactory);
            connection.Open();

            return connection;
        }
    }
}
