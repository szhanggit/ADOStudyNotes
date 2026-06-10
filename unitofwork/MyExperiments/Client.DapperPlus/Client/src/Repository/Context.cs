using Core;
using System.Data;
using System.Transactions;
using Z.Dapper.Plus;

namespace Repository
{
    public class Context : DapperPlusContext, IContextProvider, IDisposable
    {
        public void Dispose()
        {
            GC.Collect();
            Connection.Close();
            Connection.Dispose();
        }

        public TransactionScope? Transaction { get; set; }

        public void SetConnection(IDbConnection conn) => Connection = conn;
    }
}
