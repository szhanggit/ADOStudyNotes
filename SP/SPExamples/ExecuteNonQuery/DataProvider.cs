using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteNonQuery
{
    public static class DataProvider
    {
        public static bool ActivateAccounts(int orderId)
        {
            bool isSuccess = false;
            var isSuccessParameter = new SqlParameter("@IsSuccess", isSuccess) { Direction = ParameterDirection.Output };

            SqlParameter[] parameters = new SqlParameter[2] { new SqlParameter("@OrderId", orderId), isSuccessParameter };

            SqlHelper.ExecuteNonQuery("spActivateAccounts", parameters);

            return (bool)isSuccessParameter.Value;
        }

        public static void InsertAccountLE(int ProgramId, string ProgramIdentityCode, string AccountNumber, int BalanceAvailable, int FinanceExpireDays, string SessionId)
        {
            SqlHelper2.ExecuteNonQuery("spCreateAccountLE",
                new SqlParameter("@ProgramId", ProgramId),
                new SqlParameter("@ProgramCode", ProgramIdentityCode) { SqlDbType = SqlDbType.VarChar },
                new SqlParameter("@AccountNumber", AccountNumber) { SqlDbType = SqlDbType.VarChar },
                new SqlParameter("@BalanceAvailable", BalanceAvailable),
                new SqlParameter("@FinanceExpireDays", FinanceExpireDays),
                new SqlParameter("@SessionId", SessionId) { SqlDbType = SqlDbType.VarChar });
        }
    }
}
