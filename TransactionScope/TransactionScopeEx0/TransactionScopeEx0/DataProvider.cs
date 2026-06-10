using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionScopeEx0
{
    public static class DataProvider
    {
        public static void AddContactType(string Name)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                conn.MO_Execute("insert into [Person].[ContactType] ([Name], ModifiedDate) values (@name, GETDATE());", new { name = Name });
            }
        }
    }
}
