using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteNonQuery2
{
    public static class DataProvider
    {
        public static int TryInsertVouchers_HiLife(int count, string userName, int productId, int balanceAvailable, long reservationBatchId, byte? cacheNode)
        {
            var resultParameter = new SqlParameter("@resultParameter", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Count", count),
                new SqlParameter("@UserName", userName),
                new SqlParameter("@ProductId", productId),
                new SqlParameter("@BalanceAvailable", balanceAvailable),
                new SqlParameter("@ReservationBatchId", reservationBatchId),
                resultParameter
            };

            if (cacheNode.HasValue)
                parameters.Add(new SqlParameter("@CacheNode", cacheNode.Value));

            SqlHelper.ExecuteNonQuery("spTryInsertVouchers_HiLife", parameters.ToArray());

            return (int)resultParameter.Value;
        }

        public static void ExecuteRedemption(Account account, Transaction transaction, TransactionResponse response, string childAccountNumber = null, int childProgramId = 0)
        {
            string result = null;
            var responseCode = new SqlParameter("@ResponseCode", SqlDbType.VarChar, 5, ParameterDirection.Output, false, 0, 0, "ResponseCode", DataRowVersion.Current, result);
            SqlHelper2.ExecuteNonQuery("spRedemption", new SqlParameter("@AccountNumber", account.AccountNumber), new SqlParameter("@ProgramId", account.ProgramId), new SqlParameter("@BalanceAvailable", account.BalanceAvailable),
                new SqlParameter("@BalanceFrozen", account.BalanceFrozen), new SqlParameter("@AccountCheckSum", account.Checksum), new SqlParameter("@ModifyVersion", account.ModifyVersion),
                new SqlParameter("@TransactionCheckSum", transaction.Checksum), new SqlParameter("@SecurityKeyId", 23), new SqlParameter("@TranLocalDateTime", transaction.TranLocalDateTime),
                new SqlParameter("@TranLocalDate", transaction.TranLocalDate), new SqlParameter("@TranCode", transaction.TranCode), new SqlParameter("@Status", account.Status),
                new SqlParameter("@LastTranOn", transaction.TranUtcDateTime), new SqlParameter("@ConsumeTime", account.ConsumeTime), new SqlParameter("@ConsumeMerchantCode", account.ConsumeMerchantCode),
                new SqlParameter("@ConsumeShopCode", account.ConsumeShopCode), new SqlParameter("@ConsumeTerminalSSN", account.ConsumeTerminalSSN), new SqlParameter("@ChildAccountNumber", childAccountNumber), new SqlParameter("@ChildProgramId", childProgramId), new SqlParameter("@MasterRedemptionTranAmount", transaction.TranAmount), responseCode);

            response.ResponseCode = responseCode.Value.ToString();
        }

        private static void ExecuteStoredProcedureWithDetail(string procedureName, params SqlParameter[] parameters)
        {
            string result = null;
            int line = 0;
            var errorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000, ParameterDirection.Output, false, 0, 0, "ErrorMessage", DataRowVersion.Current, result);
            var errorLine = new SqlParameter("@ErrorLine", SqlDbType.Int, int.MaxValue, ParameterDirection.Output, false, 0, 0, "ErrorLine", DataRowVersion.Current, line);

            var sqlParameters = new List<SqlParameter> { errorMessage, errorLine };

            if (parameters != null)
            {
                sqlParameters.AddRange(parameters);
            }

            SqlHelper.ExecuteNonQuery(procedureName, sqlParameters.ToArray());

            if (!string.IsNullOrEmpty(errorMessage.Value.ToString()))
            {
                throw new InvalidOperationException(string.Format("Message: {0}; Error Line: {1}", errorMessage.Value, errorLine.Value ?? 0));
            }
        }
    }
}
