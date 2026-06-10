using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace DapperTXC.Core
{
    public sealed class DapperOperation : IDapperOperation
    {
        public async Task<SqlMapper.GridReader> ProcessMultipleResultSet(IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            return await dbConnection.QueryMultipleAsync(commandDefinition);
        }

        public async Task<TOut> ProcessSql<TExecute, TOut>(IDbConnection dbConnection, CommandDefinition commandDefinition)
            where TExecute : IExecuteOperation<TOut>, new()
        {
            var execute = new TExecute();
            return await execute.Execute(dbConnection, commandDefinition);
        }

        public async Task<TOut> ProcessSql<TExecute, TOut>(IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null) where TExecute : IExecuteOperation<TOut>, new()
        {
            var execute = new TExecute();
            return await execute.Execute(dbConnection, sql, param, transaction, commandTimeout, commandType);
        }
    }
}
