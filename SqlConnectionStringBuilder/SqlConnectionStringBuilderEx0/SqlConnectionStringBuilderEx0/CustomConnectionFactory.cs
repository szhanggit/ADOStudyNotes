using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionStringBuilderEx0
{
    public class CustomConnectionFactory : IDbConnectionFactory
    {
        public DbConnection CreateConnection(string nameOrConnectionString)
        {
            var name = nameOrConnectionString
              .Split('.').Last()
              .Replace("Context", string.Empty);

            var builder = new SqlConnectionStringBuilder
            {
                DataSource = @"LAPTOP-LDKKTL6G\STEVENZSERVER",
                InitialCatalog = "AdventureWorks",
                UserID = "steven",
                Password = "steven",
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };

            return new SqlConnection(builder.ToString());
        }
    }
}
