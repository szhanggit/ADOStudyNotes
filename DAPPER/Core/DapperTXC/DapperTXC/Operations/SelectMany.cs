using Dapper;
using DapperTXC.Core;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperTXC.Operations
{
    public class SelectMany<TOut> : IExecuteOperation<IEnumerable<TOut>>
    {
        public async Task<IEnumerable<TOut>> Execute(IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            return await dbConnection.QueryAsync<TOut>(commandDefinition);
        }

        public async Task<IEnumerable<TOut>> Execute(IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await dbConnection.QueryAsync<TOut>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
