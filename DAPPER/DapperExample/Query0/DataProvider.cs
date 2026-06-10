using ADOAccess;
using Dapper;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query0
{
    public static class DataProvider
    {
        public static List<int> GetOrderLineIds(string RCN)
        {
            // TODO: convert to SP?
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return (List<int>)conn.Query<int>(SqlStringManager.GetOrderLineIds, new { @RCN = RCN });
            }
        }
    }
}
