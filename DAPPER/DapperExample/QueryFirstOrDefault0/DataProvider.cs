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

namespace QueryFirstOrDefault0
{
    public static class DataProvider
    {
        public static int GetClientQuotationProductId(string productCode, string projectCode)
        {
            // TODO: convert to SP?
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return (int)conn.QueryFirstOrDefault<int>(SqlStringManager.GetClientQuotationProductId, new { @ProductCode = productCode, @ProjectCode = projectCode });
            }
        }
    }
}
