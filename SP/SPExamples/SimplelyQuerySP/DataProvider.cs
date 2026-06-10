using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplelyQuerySP
{
    public static class DataProvider
    {
        public static DataRow GetVoucherInfoByAliasSP(string ShortUrl)
        {
            SqlParameter[] parameters = new SqlParameter[] {
                new SqlParameter("@ShortUrl", ShortUrl)};

            var result = SqlHelper.ExecuteQuery("spGetVoucherByAlias2", parameters);
            return result != null && result.Tables.Count > 0 && result.Tables[0].Rows.Count > 0 ? result.Tables[0].Rows[0] : null;
        }
    }
}
