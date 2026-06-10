using ADOAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_QuerySP
{
    public static class DataProvider
    {
        public static void TryGetPartialVoucherSN(DiveTask task)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var param = new { orderLineId = task.OrderLineId, startOrderLineSN = task.StartOrderLineSN, endOrderLineSN = task.EndOrderLineSN };
                IDictionary<string, object> dic = conn.MO_Query("spTryGetPartialVoucherSN", param, commandType: CommandType.StoredProcedure).First() as IDictionary<string, object>;
                task.StartOrderLineSN = Convert.ToInt32(dic["startOrderLineSN"]);
                task.EndOrderLineSN = Convert.ToInt32(dic["endOrderLineSN"]);
            }
        }


        public static IEnumerable<VoucherToCheck> GetAdoraPieceEndVoucherNumberByOrderLine(int orderId, int orderLineId, int startOrderLineSN, int endOrderLineSN, bool isBuffer)
        {
            if (isBuffer)
            {
                using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
                {
                    return conn.MO_Query<VoucherToCheck>("spGetAdoraPieceEndVoucherNumberByOrderLineBuffer", new { @OrderId = orderId, @OrderLineId = orderLineId, @StartOrderLineSN = startOrderLineSN, @EndOrderLineSN = endOrderLineSN }, commandType: CommandType.StoredProcedure);
                }
            }
            else
            {
                using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
                {
                    return conn.MO_Query<VoucherToCheck>("spGetAdoraPieceEndVoucherNumberByOrderLine", new { @OrderId = orderId, @OrderLineId = orderLineId, @StartOrderLineSN = startOrderLineSN, @EndOrderLineSN = endOrderLineSN }, commandType: CommandType.StoredProcedure);
                }
            }
        }
    }
}
