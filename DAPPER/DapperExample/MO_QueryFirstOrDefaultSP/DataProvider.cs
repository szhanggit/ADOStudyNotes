using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_QueryFirstOrDefaultSP
{
    public static class DataProvider
    {
        public static ContentTagValue QueryFixFootNote(int businessTypeId, bool? needTrustAccount, int orderLineId)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var param = new { BusinessTypeId = businessTypeId, NeedTrustAccount = needTrustAccount, OrderLineId = orderLineId };
                return conn.MO_QueryFirstOrDefault<ContentTagValue>("spQueryFixFootNote", param, commandType: CommandType.StoredProcedure);
            }
        }

        public static ClientOrderLine GetEMVClientOrderLine(int id)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_QueryFirstOrDefault<ClientOrderLine>("spGetEMV_ClientOrderLine", new { @EMV_ClientOrderLineId = id }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
