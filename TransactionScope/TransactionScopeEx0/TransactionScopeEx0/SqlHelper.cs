using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionScopeEx0
{
    public static class SqlHelper
    {
        private static string _connectionStringName = "AdventureWorksDB";

        public static string MOConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            }
        }
    }
}
