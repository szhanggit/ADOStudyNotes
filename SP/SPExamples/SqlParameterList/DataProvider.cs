using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlParameterList
{
    public static class DataProvider
    {
        public static DataTable QueryVouchers(string voucherNo, string clientOrderNo, string email, string mobile, string shortUrl, string guid, DateTime? activeDate, int? voucherStatus, int userId, int pageIndex, int pageSize)
        {
            var @params = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(voucherNo))
            {
                @params.Add(new SqlParameter("@VoucherNumber", voucherNo));
            }

            if (!string.IsNullOrEmpty(clientOrderNo))
            {
                @params.Add(new SqlParameter("@ClientOrderNo", clientOrderNo));
            }

            if (!string.IsNullOrEmpty(email))
            {
                @params.Add(new SqlParameter("@Email", email));
            }

            if (!string.IsNullOrEmpty(mobile))
            {
                @params.Add(new SqlParameter("@Mobile", mobile));
            }

            if (!string.IsNullOrEmpty(shortUrl))
            {
                @params.Add(new SqlParameter("@ShortUrl", shortUrl));
            }

            if (!string.IsNullOrEmpty(guid))
            {
                @params.Add(new SqlParameter("@GUID", guid));
            }

            if (activeDate.HasValue)
            {
                @params.Add(new SqlParameter("@ActiveDate", activeDate));
            }

            if (voucherStatus.HasValue)
            {
                @params.Add(new SqlParameter("@VoucherStatus", voucherStatus));
            }

            @params.Add(new SqlParameter("@CSUserId", userId));
            @params.Add(new SqlParameter("@PageIndex", pageIndex));
            @params.Add(new SqlParameter("@PageSize", pageSize));

            var result = SqlHelper.ExecuteQuery("spCSSQueryVouchers", @params.ToArray());

            return result != null && result.Tables.Count > 0 ? result.Tables[0] : null;
        }
    }
}
