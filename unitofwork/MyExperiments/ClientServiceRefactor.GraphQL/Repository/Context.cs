using Core;
using System.Data;
using System.Transactions;

namespace Repository
{
    public class Context : IContextProvider, IDisposable
    {
        public void Dispose()
        {
            GC.Collect();
            Connection.Close();
            Connection.Dispose();
        }

        public IDbConnection Connection { get; set; }
        public TransactionScope? Transaction { get; set; }

        public void SetConnection(IDbConnection conn) => Connection = conn;
    }
}
