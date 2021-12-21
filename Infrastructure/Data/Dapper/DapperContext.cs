using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Data.Dapper
{
    public class DapperContext : IDapperContext
    {
        private readonly string _connectionString;
        public DapperContext(string connectionString)
        {
            _connectionString =
                connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
