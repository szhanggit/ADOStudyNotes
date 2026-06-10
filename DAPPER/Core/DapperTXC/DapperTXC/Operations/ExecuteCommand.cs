using Dapper;
using DapperTXC.Core;
using System.Data;
using System.Threading.Tasks;

namespace DapperTXC.Operations
{
    public class ExecuteCommand : IExecuteOperation<int>
    {

        public async Task<int> Execute(IDbConnection dbConnection, CommandDefinition commandDefinition)
        {
            return await dbConnection.ExecuteAsync(commandDefinition);
        }

        public async Task<int> Execute(IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return await dbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
