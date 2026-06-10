using Dapper;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DapperTXC.Core
{
    public interface IDapperOperation
    {
        Task<TOut> ProcessSql<TExecute, TOut>(IDbConnection dbConnection, CommandDefinition commandDefinition)
            where TExecute : IExecuteOperation<TOut>, new();
        Task<TOut> ProcessSql<TExecute, TOut>(IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
            where TExecute : IExecuteOperation<TOut>, new();

        Task<GridReader> ProcessMultipleResultSet(IDbConnection dbConnection, CommandDefinition commandDefinition);
    }
}
