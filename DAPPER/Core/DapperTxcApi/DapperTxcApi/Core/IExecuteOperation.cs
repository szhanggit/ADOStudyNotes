using Dapper;
using System.Data;
using System.Threading.Tasks;

namespace DapperTxcApi.Core
{
    public interface IExecuteOperation<TOut>
    {
        //public Task<TOut> Execute(IDapperParameter parameter);
        public Task<TOut> Execute(IDbConnection dbConnection, CommandDefinition commandDefinition);
        public Task<TOut> Execute(IDbConnection dbConnection, string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
