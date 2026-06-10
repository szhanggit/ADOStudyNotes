using ADOAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteSP
{
    public static class DataProvider
    {
        public static void ReverseTrashVoucherCredit(long batchlogId)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                conn.Execute("spReverseTrashVoucherCredit", new { batchlogId }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
