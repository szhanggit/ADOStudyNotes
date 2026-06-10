using ADOAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_QueryFirstOrDefault0
{
    public static class DataProvider
    {
        public static VoucherAndProductInfo GetMasterVoucherAndProductByComboChildVoucher(long childVoucherId)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                VoucherAndProductInfo ret = conn.MO_QueryFirstOrDefault<VoucherAndProductInfo>(SqlStringManager.GetMasterVoucherAndProductByComboChildVoucher, new { ChildVoucherId = childVoucherId });
                return ret;
            }
        }


        public static bool IsChildVoucher(long voucherId)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                bool flag = conn.MO_QueryFirstOrDefault<bool>(SqlStringManager.IsChildVoucher, new { VoucherId = voucherId });
                return flag;
            }
        }

        public static VoucherSupplierAPILog_TPC QueryVoucherSupplierAPILog_TPC(string requestId)
        {
            string sql = @"SELECT TOP 1 [RequestId],[CreatedOn],[ResponseCode] FROM dbo.VoucherSupplierAPILog_TPC WITH(NOLOCK) WHERE RequestId = @RequestId ORDER BY id desc";

            using (var connection = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return connection.MO_QueryFirstOrDefault<VoucherSupplierAPILog_TPC>(sql, new { RequestId = new DbString() { Value = requestId, IsAnsi = true } });
            }
        }





        public static BatchTranVoucherDTO GetBatchTranVoucherInfo(string queryVal, int programId, string firstColType)
        {
            var whereClause = "V.VoucherNumber= @QueryVal";
            if (firstColType == "alias")
            {
                whereClause = "V.ShortUrl= @QueryVal";
            }
            else if (firstColType == "guid")
            {
                whereClause = "V.Guid = CONVERT(uniqueidentifier,@QueryVal)";
            }

            //MGCType(AccountIdentityType): 2:Master Voucher 4:Non Nativer Voucher 5:Non Native Child voucher 1:General Vocher; 3:Native Child Voucher
            //MultipleSelectionType: 0: General , 1: Master, 2: Child
            string sql = @"
                        SELECT CASE
                            WHEN p.IsMasterProduct = 1
                            THEN 2
		                    WHEN p.IsMasterProduct = 0 AND VC.ChildVoucherId IS NULL AND p.MultipleSelectionType =1
                            THEN 4
		                    WHEN VC.ChildVoucherId IS NULL AND p.MultipleSelectionType = 2
		                    THEN 5
                            WHEN VC.ChildVoucherId IS NULL
                            THEN 1
                            ELSE 3
                        END AS VoucherMGCType,
                        V.Id VoucherId,
                        V.[Status] VoucherStatus,
                        P.Id ProductId,
                        p.ProductType,
                        P.ProductSubType
                    FROM Voucher V WITH(NOLOCK)
                        INNER JOIN Product P WITH(NOLOCK) ON V.ProductId = P.Id
                        LEFT JOIN VoucherCombo vc WITH(NOLOCK) ON v.Id = vc.ChildVoucherId
                                                                AND vc.[Status] = 1
                        Where ( " + whereClause + @" )
                              AND V.ProgramId = @programId
                        ";
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_QueryFirstOrDefault<BatchTranVoucherDTO>(sql, new { QueryVal = new DbString() { Value = queryVal, IsAnsi = true }, programId });
            }
        }



        public static long BuildEMVDiveTask(long orderId, long orderLineId, int endOrderLineSN, string session)
        {
            string sql = @"INSERT INTO [dbo].[DiveTask]
           ([OrderId]
           ,[OrderLineId]
           ,[StartOrderLineSN]
           ,[EndOrderLineSN]
           ,[Status]
           ,[CreationTime]
           ,[SessionId])
                 VALUES
                       (@OrderId
                       ,@OrderLineId
                       ,1
                       ,@EndOrderLineSN
                       ,2
                       ,GETDATE()
                       ,@Session)

            Select SCOPE_IDENTITY();";

            using (var connection = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return connection.MO_QueryFirstOrDefault<long>(sql, new { OrderId = orderId, OrderLineId = orderLineId, EndOrderLineSN = endOrderLineSN, Session = session });
            }
        }


        public static TranMinMaxId GetTranMinMaxId(long TranLocalDate)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var sql = $"SELECT min(t1.Id) as MinId, max(t1.Id) as MaxId " +
                    $"FROM dbo.[Transaction] t1 with(nolock) JOIN dbo.Dictionary t2 with(nolock) " +
                    $"ON t1.TranType = t2.Name AND t2.Category = 'TransactionType' " +
                    $"LEFT JOIN dbo.Merchant t3 with(nolock) ON t1.MerchantId = t3.Id " +
                    $"LEFT JOIN dbo.Shop t4 with(nolock) ON t1.ShopId = t4.Id " +
                    $"LEFT JOIN dbo.Product t5 with(nolock) ON t1.ProductId = t5.Id " +
                    $"LEFT JOIN dbo.[Order] t6 with(nolock) ON t1.OrderId = t6.Id " +
                    $"LEFT JOIN dbo.OrderLine t7 with(nolock) ON t7.OrderId = t6.Id AND t1.ClientQuotationProductId = t7.ClientQuotationProductId " +
                    $"LEFT JOIN dbo.ClientQuotationProduct t8 with(nolock) ON t1.ClientQuotationProductId = t8.Id " +
                    $"LEFT JOIN dbo.Voucher t9 with(nolock) ON t1.AccountNumber = t9.VoucherNumber " +
                    $"INNER JOIN dbo.Program t10 with(nolock) ON t1.ProgramCode = t10.IdentityCode AND t9.ProgramId = t10.Id " +
                    $"LEFT JOIN dbo.OrderBeneficiaryInfo t11 with(nolock) ON t9.BeneficiaryInfoId = t11.Id " +
                    $"LEFT JOIN [dbo].[ProductVersion] t12 with(nolock) ON t8.ProductVersionId = t12.Id " +
                    $"LEFT JOIN [dbo].[ProductPricingProductBased] T13 with(nolock) ON T12.ProductPricingProductBasedId = T13.Id " +
                    $"LEFT JOIN [dbo].[ProductPricingValueBased] T14 with(nolock) ON T12.ProductPricingValueBasedId = T14.Id " +
                    $"LEFT JOIN [dbo].ProductPricingDynamicFaceValue T17 with(nolock) ON T17.ProductVersionId = t12.Id " +
                    $"LEFT JOIN [dbo].[ClientQuotationProductSoldPrice] t15 with(nolock) " +
                    $"ON t8.Id = t15.[ClientQuotationProductId] AND t15.ValidityFrom <= t11.ActiveDate " +
                    $"AND t15.ValidityEnd >= t11.ActiveDate AND T15.Status = 2 " +
                    $"LEFT JOIN [dbo].[ReimbursementScheme] T16 with(nolock) " +
                    $"ON T16.Id= T1.[ReimbursementSchemeId] AND T16.TransactionTypeId = t2.Id " +
                    $"WHERE t1.TranLocalDate = {TranLocalDate} AND T1.ResponseCode = '0000'";
                return conn.MO_QueryFirstOrDefault<TranMinMaxId>(sql, null, int.MaxValue);
            }
        }
    }
}
