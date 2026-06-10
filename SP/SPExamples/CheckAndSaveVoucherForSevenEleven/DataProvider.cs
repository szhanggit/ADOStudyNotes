using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckAndSaveVoucherForSevenEleven
{
    public static class DataProvider
    {
        public static DataTable CheckAndSaveVoucherForSevenEleven(string sessionId)
        {
            var result = SqlHelper.ExecuteQuery("spCheckAndSaveVoucherForSevenEleven", new SqlParameter("@SessionId", sessionId));
            if (result != null && result.Tables.Count > 0)
                result.Tables[0].TableName = "errorDataTable";
            else
                result.Tables.Add(new DataTable("resultData"));
            return result.Tables[0];
        }
    }
}
