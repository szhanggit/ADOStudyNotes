using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetOutputValue
{
    public static class DataProvider
    {
        public static DataTable CheckAndGetAPIOrderInfo(string orderNumber, string productCode, string consumerCode, int requestQuanity, out string responseCode, out bool isIgnoreIssueNumberLimit, Decimal? soldPrice = null, Decimal? soldPriceWithTax = null)
        {
            string ret = string.Empty;
            var RetParameter = new SqlParameter("@ResponseCode", SqlDbType.VarChar, 10, ParameterDirection.Output, false, 0, 0, "ret", DataRowVersion.Current, ret);
            bool retIsIgnore = false;
            var retIsIgnoreIssueNumberLimit = new SqlParameter("@IsIgnoreIssueNumberLimit", retIsIgnore) { Direction = ParameterDirection.Output, DbType = DbType.Boolean };
            List<SqlParameter> parameters =
            new List<SqlParameter>()
                {
                    new SqlParameter("@OrderNumber", orderNumber),
                    new SqlParameter("@ProductCode", productCode),
                    new SqlParameter("@ConsumerCode", consumerCode),
                    new SqlParameter("@RequestQuanity", requestQuanity),
                    RetParameter,
                    retIsIgnoreIssueNumberLimit
                  };
            if (soldPrice.HasValue)
            {
                parameters.Add(new SqlParameter("@SoldPrice", soldPrice.Value));
            }
            if (soldPriceWithTax.HasValue)
            {
                parameters.Add(new SqlParameter("@SoldPriceWithTax", soldPriceWithTax.Value));
            }

            var ds = SqlHelper.ExecuteQuery("spCheckAndGetAPIOrderInfo", parameters.ToArray());

            responseCode = RetParameter.Value.ToString();

            isIgnoreIssueNumberLimit = "0000".Equals(responseCode) ? (bool)retIsIgnoreIssueNumberLimit.Value : false;


            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }
    }
}
