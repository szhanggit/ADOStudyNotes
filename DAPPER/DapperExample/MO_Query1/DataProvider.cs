using ADOAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_Query1
{
    public static class DataProvider
    {
        
        

        public static IList<OrderDetailActionHistoryDataModel> GetOrderDetailActionHistory(int orderId)
        {
            string sql = @"SELECT Id, OrderId, ActionType, ActionResult, Operator, ActionTime, EmailReceiver FROM dbo.OrderDetailActionHistory WITH(NOLOCK) WHERE OrderId = @OrderId";

            using (var connection = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return connection.MO_Query<OrderDetailActionHistoryDataModel>(sql, new { OrderId = orderId }).ToList();
            }
        }

        public static IEnumerable<PreIssuedCampaignVoucherRequestInfo> GetPreIssuedCampaignVoucherRequestInfos()
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var sql = @"SELECT [Id]
                      ,[OrderNumber]
                      ,[OrderBeneficiaryInfoId]
                      ,[ProductCode]
                      ,[VoucherQuantity]
                      ,[CreatedOn]
                      ,[CreatedBy]
	                  ,[ProcessStatus]   
                  FROM [dbo].[PreIssuedCampaignVoucherRequestInfo] with(nolock)";
                return conn.MO_Query<PreIssuedCampaignVoucherRequestInfo>(sql);
            }
        }

        public static int TryInsertCampaignVouchersFromStock(int count, int programId, int orderLineId, byte? cacheNode, bool needChangeAuthCode, long orderBeneficiaryInfoId, string sessionId)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var param = new { count = count, orderLineId = orderLineId, cacheNode = cacheNode, needChangeAuthCode = needChangeAuthCode, orderBeneficiaryInfoId = orderBeneficiaryInfoId, sessionId = sessionId };
                return conn.MO_Query<int>(SqlStringManager.TryInsertCampaignVouchersFromStock(CommonUtility.GetVoucherStockTableName(programId), CommonUtility.GetVoucherStockPrefix(programId)), param, commandTimeout: DapperHelper.LongTimeout).FirstOrDefault();
            }
        }

        public static int AddClearCacheProductStopAPI(int productId, DateTime validEnd)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var param = new { ProductId = productId, ValidEnd = validEnd };
                return conn.MO_Query<int>(@"insert into ClearCacheProductStopAPI (ProductId , [Status],ValidEnd) Values(@ProductId,0,@ValidEnd);
                                            select SCOPE_IDENTITY()", param, commandTimeout: DapperHelper.LongTimeout).FirstOrDefault();
            }
        }



        public static int TrashVoucherInInventory(string VoucherNumber, string ProgramCode)
        {
            int result = -1;
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                result = conn.MO_Query<int>(SqlStringManager.TrashVoucherInInventory(CommonUtility.GetVoucherStockTableNameWithProgramCode(ProgramCode), VoucherNumber)).Single();
            }
            return result;
        }



        public static IDictionary<string, object> QueryStockCountAndLock_GR(int expectCount, bool canLessThanExpect, int OrderLineId, int programId, int productId, int reservationCodeId)
        {
            IDictionary<string, object> dic;
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var param = new { expectCount = expectCount, canLessThanExpect = canLessThanExpect, OrderLineId = OrderLineId, productId = productId, reservationCodeId = reservationCodeId };
                dic = conn.MO_Query(SqlStringManager.QueryStockCountAndLock_GR(CommonUtility.GetVoucherStockTableName(programId)), param).FirstOrDefault() as IDictionary<string, object>;

            }
            return dic;
        }



        public static void INImportedThirdPartyProductStat()
        {
            string errorMessage = null;
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                errorMessage = conn.MO_Query<string>(SqlStringManager.INImportedThirdPartyProductStat(), commandTimeout: DapperHelper.MiddleTimeout).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new InvalidOperationException(string.Format("Message: {0}; Error Line: {1}", errorMessage, 0));
            }
        }



        public static bool CheckBeneficiaryEmailOrMobile(int orderLineId, int OrderLineStartSN, int OrderLineEndSN, bool isEmail = true, string sessionId = null)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var param = new { orderLineId = orderLineId, OrderLineStartSN = OrderLineStartSN, OrderLineEndSN = OrderLineEndSN };
                return conn.MO_Query<bool>(SqlStringManager.CheckBeneficiaryEmailOrMobile(isEmail, sessionId), param).FirstOrDefault();
            }
        }


    }
}
