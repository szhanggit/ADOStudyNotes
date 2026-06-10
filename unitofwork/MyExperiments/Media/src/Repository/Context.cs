
using System.Data;
using System.Transactions;
using TXC.Common.RepositoryCore;
using Microsoft.Data.SqlClient;

namespace Repository
{
    public class Context :  IContextProvider
    {

        public void Dispose()
        {
            GC.Collect();
            Connection.Close();
            Connection.Dispose();
        }
        public IDbConnection Connection { get; set; }
        public TransactionScope? Transaction { get; set; }

        public void SetConnection(string connectionString) => Connection = new SqlConnection(connectionString);

        public void SetConnection(IDbConnection connection) => Connection = connection;
    }
}
