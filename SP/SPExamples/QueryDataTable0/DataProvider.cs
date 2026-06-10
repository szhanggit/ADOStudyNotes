using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueryDataTable0
{
    public static class DataProvider
    {
        public static DataTable LoadAcceptanceLoop(int id)
        {
            return SqlHelper2.QueryDataTable("spGetAcceptanceLoopById", new SqlParameter("@Id", id));
        }

        public static Transaction FindTransactionByTerminalSSN(string accountNumber, string terminalSSN, int originalTransactionDate, out bool isHistory)
        {
            Transaction transaction = null;
            var outParam = new SqlParameter("isHistory", false) { Direction = ParameterDirection.Output };

            var dataTable = SqlHelper2.QueryDataTable("spFindTransactionByTerminalSSN",
                                    new SqlParameter("@AcountNumber", accountNumber) { SqlDbType = SqlDbType.VarChar },
                                    new SqlParameter("@TerminalSSN", terminalSSN) { SqlDbType = SqlDbType.VarChar },
                                    new SqlParameter("@OriginalTransactionDate", originalTransactionDate),
                                    outParam);
            isHistory = (bool)outParam.Value;

            if (dataTable != null && dataTable.Rows.Count == 1)
            {
                transaction = ConstructTransaction(accountNumber, dataTable.Rows[0]["TranCode"].ToString(), dataTable.Rows[0]);
            }

            return transaction;
        }

        private static Transaction ConstructTransaction(string accountNumber, string tranCode, DataRow dataRow)
        {
            return new Transaction
            {
                TranUtcDateTime = dataRow.Field<DateTime>("TranUtcDateTime"),
                TranType = dataRow.Field<short>("TranType"),
                Status = dataRow.Field<byte>("Status"),
                TranAmount = dataRow.Field<int>("TranAmount"),
                TranCodeRef = dataRow.Field<string>("TranCodeRef"),
                RefundedAmount = dataRow.Field<int>("RefundedAmount"),
                ResponseCode = dataRow.Field<string>("ResponseCode"),
                ModifyVersion = dataRow.Field<int?>("ModifyVersion"),
                BalanceAvailable = dataRow.Field<int?>("BalanceAvailable"),
                BalanceFrozen = dataRow.Field<int?>("BalanceFrozen"),
                Checksum = dataRow.Field<string>("Checksum"),
                SecurityKeyId = dataRow.Field<int?>("SecurityKeyId"),
                AccountNumber = accountNumber,
                TranCode = tranCode,
            };
        }

        public static DataTable GenerateAccounts(int number, int digitNumber)
        {
            SqlParameter[] parameters = new SqlParameter[]{
                     new SqlParameter("@cnt",number),
                     new SqlParameter("@digitNumber",digitNumber)

            };
            return SqlHelper2.QueryDataTable("pf_generate_code", parameters);
        }

        public static DataTable FindAccounts(string accountNumber, string programCode, string order, int accountStatus, DateTime createdOnFrom, DateTime createdOnTo, DateTime lastTranOnFrom, DateTime LastTranOnTo)
        {
            var parameterList = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(accountNumber))
            {
                parameterList.Add(new SqlParameter("@AcountNumber", accountNumber));
            }

            if (!string.IsNullOrEmpty(programCode))
            {
                parameterList.Add(new SqlParameter("@ProgramCode", programCode));
            }

            if (!string.IsNullOrEmpty(order))
            {
                parameterList.Add(new SqlParameter("@ExternalOrderId", order));
            }

            if (accountStatus > 0 && accountStatus < 64)
            {
                parameterList.Add(new SqlParameter("@Status", accountStatus));
            }

            if (createdOnFrom >= DateTime.MinValue.SqlMinValue())
            {
                parameterList.Add(new SqlParameter("@CreatedOnFrom", createdOnFrom));
            }

            if (createdOnTo >= DateTime.MinValue.SqlMinValue())
            {
                parameterList.Add(new SqlParameter("@CreatedOnTo", createdOnTo));
            }

            if (lastTranOnFrom >= DateTime.MinValue.SqlMinValue())
            {
                parameterList.Add(new SqlParameter("@LastTranOnFrom", lastTranOnFrom));
            }

            if (LastTranOnTo >= DateTime.MinValue.SqlMinValue())
            {
                parameterList.Add(new SqlParameter("@LastTranOnTo", LastTranOnTo));
            }

            return SqlHelper2.QueryDataTable("spFindAccounts", parameterList.ToArray());
        }

        public static DataTable QueryAccountToSync(string programCode, long startChangedTime, long endChangedTime, int totalRecords, int? status = null)
        {
            SqlParameter[] parameters = new SqlParameter[5] { new SqlParameter("@StartChangedUTCTimeL", startChangedTime)
                                                            , new SqlParameter("@EndChangedUTCTimeL", endChangedTime)
                                                            , new SqlParameter("@ProgramCode", programCode)
                                                            , new SqlParameter("@Count", totalRecords)
                                                            , new SqlParameter("@Status", status.HasValue?status.Value:(object)DBNull.Value)};

            return SqlHelper2.QueryDataTable("spGetAccountToSync", parameters);
        }


        public static AuthorizationEntityInfo GetTerminalEntityInfo(string identityCode, int programId)
        {
            AuthorizationEntityInfo resultEntity = null;


            var result = SqlHelper2.QueryDataTable("spGetTerminalEntityInfo", new SqlParameter("@IdentityCode", identityCode) { SqlDbType = SqlDbType.VarChar, Size = 50 }, new SqlParameter("@ProgramId", programId));

            if (result != null && result.Rows.Count > 0)
            {
                var row = result.Rows[0];
                resultEntity = new AuthorizationEntityInfo();
                resultEntity.SecurityKeyId = (int?)row[0];
                resultEntity.TimeOffset = (string)row[1];
                resultEntity.MaxFailNumber = (int)row[2];
            }

            return resultEntity;
        }
    }
}
