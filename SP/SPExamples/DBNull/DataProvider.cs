using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBNull
{
    public static class DataProvider
    {
        public static DataTable GetPreIssuedCampaignVoucher_SevenEleven(string orderNumber, int productId, int reservationCodeId, byte cacheNodeId, string session)
        {

            var result = SqlHelper.ExecuteQuery("spGetPreIssuedCampaignVoucher_SevenEleven",
                  new SqlParameter("@ProductId", productId),
                  new SqlParameter("@ReservationCodeId", reservationCodeId),
                  new SqlParameter("@CacheNodeId", cacheNodeId),
                  new SqlParameter("@OrderNumber", orderNumber),
                  new SqlParameter("@Session", string.IsNullOrEmpty(session) ? (object)System.DBNull.Value : session));

            return result != null && result.Tables.Count > 0 ? result.Tables[0] : null;
        }
    }
}
