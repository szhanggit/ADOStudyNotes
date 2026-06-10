using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LikeQuery
{
    public static class DataProvider
    {
        public static int GetProgramIdentityCode()
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return Int32.Parse(conn.MO_QueryFirstOrDefault<string>(@"SELECT TOP (1) [IdentityCode] FROM [dbo].[Program]
                            where IdentityCode like '0____'
                            order by Id desc"));
            }
        }
    }
}
