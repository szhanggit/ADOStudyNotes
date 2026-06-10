using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TXC.Common.RepositoryCore;
using Z.Dapper.Plus;

namespace RepositoryDapper
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

        public void SetConnection(string connectionString) => Connection.ConnectionString = connectionString;
    }
}
