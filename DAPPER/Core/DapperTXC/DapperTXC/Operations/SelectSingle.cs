using Dapper;
using DapperTXC.Core;
using System.Data;
using System.Threading.Tasks;

namespace DapperTXC.Operations
{
    public class SelectSingle<TOut> : IExecuteOperation<TOut>
    {

        public async Task<TOut> Execute(IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            return await dbConnection.QueryFirstOrDefaultAsync<TOut>(commandDefinition);
        }

        public async Task<TOut> Execute(IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await dbConnection.QueryFirstOrDefaultAsync<TOut>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
