using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParametersAddWithValue
{
    public static class DataProvider
    {
        public static DataTable QueryTransFromOlapDB(int step)
        {
            DataTable dt = new DataTable();

            string sql = @"SELECT TOP (@Step)       [Id]
                          ,[AccountNumber]
                          ,[ProgramCode]
                          ,[MerchantCode]
                          ,[ShopCode]
                          ,[TranType]
                          ,[TranLocalDate]
                          ,[TranLocalDateTime]
                          ,[SettleDate]
                          ,[TranRealDateTime]
                          ,[Status]
                          ,[TranAmount]
                          ,[RefundedAmount]
                          ,[ResponseCode]
                          ,[Channel]
                          ,[TranCode]
                          ,[TranCodeRef]
                          ,[BalanceAvailable]
                          ,[BalanceFrozen]
                          ,[MerchantId]
                          ,[ShopId]
                          ,[ClientQuotationProductId]
                          ,[ProductId]
                          ,[OrderId]
                          ,[InvoiceToBeRequestedId]
                          ,[ClientQuotationPricingId]
                          ,[ReimbursementSchemeId]
                          ,[ReimbursementLineId]
                          ,[InvoiceValue]
                          ,[ReimbursementValue]
                          ,[Bill]
                          ,[ServiceFee]
                          ,[IIV]
                          ,[IV]
                          ,[Discount]
                          ,[IRV]
                          ,[RV]
                          ,[NWR]
                          ,[NWA]
                          ,[IEV]
                          ,[EV]
                          ,[NoShowPrice]
                          ,[NoShowCost]
                          ,[CostMarketing]
                          ,[CostEmployeeWelfare]
                          ,[CostTest]
                          ,[Rsv1]
                          ,[Rsv2]
                          ,[Rsv3]
                          ,[TerminalSSN]
                          ,[ShiftCode]
                          ,[BusinessDay]
                          ,[TerminalCode]
                          ,[FNDiscount]
                          ,[FNNWA]
                          ,[FNEV]
                          ,[FNEVWithhold]
                          ,[FNEVDiscount]
                          ,[FNEVWithholdDiscount]
                    FROM dbo.[Transaction] ORDER BY Id";

            using (SqlDataAdapter adapter = new SqlDataAdapter(sql, SqlHelper.MoveOlapConnectionString))
            {
                adapter.SelectCommand.CommandTimeout = 5 * 60;//5 mins
                adapter.SelectCommand.Parameters.AddWithValue("@Step", step);
                adapter.Fill(dt);
            }
            return dt;
        }
    }
}
