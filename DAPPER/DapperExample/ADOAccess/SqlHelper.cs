using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOAccess
{
    public static class SqlHelper
    {
        private static string _connectionStringName = "MODB";

        public static string MOConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            }
        }
    }
}
