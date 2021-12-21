using System.Data;

namespace Infrastructure.Data.Dapper
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }
}
