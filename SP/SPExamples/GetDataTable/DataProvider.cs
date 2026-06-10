using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataTable
{
    public static class DataProvider
    {
        public static DataTable GetAutoCalculateReimbursementMerchant()
        {
            var result = SqlHelper.ExecuteQuery("spGetAutoCalculateReimbursementMerchant");

            return result != null && result.Tables.Count > 0 ? result.Tables[0] : null;
        }
    }
}
