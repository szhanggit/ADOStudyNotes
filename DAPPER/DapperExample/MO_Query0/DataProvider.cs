using ADOAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_Query0
{
    public static class DataProvider
    {
        private static ConcurrentDictionary<int, IDictionary<string, object>> voucherStockInfoDictionary = new ConcurrentDictionary<int, IDictionary<string, object>>();//缓存
        public static int UpdateVoucherStatusForClearCache(
            int ProductId,
            string AvailableStartDate,
            string AvailableEndDate,
            string ExpiryDate,
            string TaskId,
            int programId,
            string ExcludeVoucherNumberListStr)
        {
            string Table = GetVoucherStockTableName(programId);
            string sql = SqlStringManager.UpdateVoucherStatusForClearCache(Table, ProductId.ToString(), AvailableStartDate, AvailableEndDate, ExpiryDate, TaskId, ExcludeVoucherNumberListStr);
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_Query<int>(sql, commandTimeout: DapperHelper.LongTimeout).FirstOrDefault();
            }
        }

        public static string GetVoucherStockTableName(int programId)
        {
            IDictionary<string, object> dic = GetVoucherStockInfo(programId);
            if (dic != null)
            {
                return dic["VoucherStockTableName"].ToString();
            }
            return null;
        }

        private static IDictionary<string, object> GetVoucherStockInfo(int programId)
        {
            if (voucherStockInfoDictionary.ContainsKey(programId))
            {
                IDictionary<string, object> value;
                voucherStockInfoDictionary.TryGetValue(programId, out value);
                return value;
            }

            IDictionary<string, object> dic;
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                dic = conn.MO_Query(SqlStringManager.QueryStockInfo, new { programId = programId }).FirstOrDefault() as IDictionary<string, object>;
            }
            if (dic != null)
            {
                voucherStockInfoDictionary.TryAdd(programId, dic);
            }
            return dic;
        }

        public static decimal CalculateOrderIssueValue(int clientQuotationId, Dictionary<int, int> orderLineInfoDic)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var list = conn.MO_Query(@"SELECT cqp.Id, CASE p.ProductType when 1 then ISNULL(cqps.SoldPriceWithTax,0) * pppb.Balance when 2 then ISNULL(cqps.ValueBasedSoldPriceWithTax,0) ELSE 0 end + SUM(ISNULL(st.SoldPriceWithTax, 0)) value
                                                FROM dbo.ClientQuotation cq WITH(NOLOCK)
                                                JOIN dbo.ClientQuotationProduct cqp WITH(NOLOCK) ON cqp.ClientQuotationId = cq.Id
                                                JOIN dbo.ProductVersion pv WITH(NOLOCK) ON pv.Id = cqp.ProductVersionId
                                                JOIN dbo.Product p WITH(NOLOCK) ON p.Id = pv.ProductId
                                                JOIN dbo.ClientQuotationProductSoldPrice cqps WITH(NOLOCK) ON cqps.ClientQuotationProductId = cqp.Id and cqps.ValidityFrom < GETDATE() and cqps.ValidityEnd > GETDATE()
                                                LEFT JOIN dbo.ProductPricingProductBased pppb WITH(NOLOCK) ON pppb.Id = pv.ProductPricingProductBasedId
                                                LEFT JOIN dbo.ClientQuotationProductServiceFeeTriggerTime st WITH(NOLOCK) ON st.ClientQuotationProductId = cqp.Id
                                                WHERE cq.Id = @clientQuotationId
                                                GROUP BY cqp.id, p.ProductType, cqps.SoldPriceWithTax, cqps.ValueBasedSoldPriceWithTax, pppb.Balance
                                                ORDER BY cqp.Id",
                                new { clientQuotationId }).ToList();

                decimal result = 0;
                foreach (var item in list)
                {
                    if (orderLineInfoDic.ContainsKey(item.Id))
                    {
                        result = result + (orderLineInfoDic[item.Id] * item.value);
                    }
                }

                return result;
            }
        }


        public static IEnumerable<EMVOrderProductInfo> GetEMVOrderProductInfoList(string orderNumber)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_Query<EMVOrderProductInfo>("SELECT RCN,FirstName,LastName,OrderBeneficiaryInfoId FROM EXT_IN_APIOrderInfo WITH(NOLOCK) WHERE OrderNumber = @orderNumber;", new { orderNumber });
            }
        }

        public static IEnumerable<QueryMerchantAndShopResult> QueryMerchantAndShop(string merchantCode, string merchantName, string shopCode, string shopName, int pageSize, int pageIndex)
        {
            var sql = @"WITH T1 AS
                (SELECT
	                m.IdentityCode AS MerchantCode,
	                m.Name AS MerchantName,
	                s.IdentityCode AS ShopCode,
	                s.Name AS ShopName,
	                s.Status,
	                p.IdentityCode AS ProgramCode
                FROM Shop s WITH (NOLOCK)
                INNER JOIN Merchant m WITH (NOLOCK) ON m.Id = s.MerchantId
                INNER JOIN Program p WITH (NOLOCK) ON p.Id = m.ProgramId
                WHERE (m.IdentityCode = @MerchantCode OR @MerchantCode IS NULL)
                AND (m.Name LIKE @MerchantName OR @MerchantName IS NULL)
                AND (s.IdentityCode = @ShopCode OR @ShopCode IS NULL)
                AND (s.Name LIKE @ShopName OR @ShopName IS NULL)),
                T2 AS (SELECT COUNT(1) Total FROM T1)
                SELECT * FROM T2, T1
                ORDER BY MerchantCode, ShopCode
                OFFSET @PageIndex * @PageSize ROWS
                FETCH NEXT @Pagesize ROWS ONLY";
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_Query<QueryMerchantAndShopResult>(sql, new { MerchantCode = merchantCode, MerchantName = merchantName, ShopCode = shopCode, ShopName = shopName, PageSize = pageSize, PageIndex = pageIndex });
            }
        }

        public static CSFactorAuthCode QueryFactorAuth(int csuserId)
        {
            string sql = @"select CsuserId, FactorAuthCode, UpdateTime, LoginTime from CSFactorAuthCode with(nolock) where CsuserId = @csuserId";

            using (var connection = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return connection.MO_Query<CSFactorAuthCode>(sql, new { csuserId }).FirstOrDefault();
            }
        }
    }
}
