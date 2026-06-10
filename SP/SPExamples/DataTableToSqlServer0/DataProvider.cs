using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DataTableToSqlServer0
{
    public static class DataProvider
    {
        public static void IssueAccount(DataTable accountTable, DataTable transactionTable, DataTable memberVoucherTable = null)
        {
            using (var transactionScope = new TransactionScope())
            {
                SqlHelper2.DataTableToSqlServer(accountTable);
                SqlHelper2.DataTableToSqlServer(transactionTable);
                if (memberVoucherTable != null)
                    SqlHelper2.DataTableToSqlServer(memberVoucherTable);
                transactionScope.Complete();
            }
        }

        public static bool BatchActivateSeqAccountList(DataTable accountBuffer3Table, string sessionId, string BusinessUnit, List<SequenceNumberInfo> updated, List<long> inserted)
        {
            bool isSuccess = false;

            DataTable dt_updated = new DataTable();
            dt_updated.Columns.Add("SequenceNumber", typeof(long));
            dt_updated.Columns.Add("CreatedUTCTime", typeof(long));

            DataTable dt_Inserted = new DataTable();
            dt_Inserted.Columns.Add("SequenceNumber", typeof(long));
            dt_Inserted.Columns.Add("CreatedUTCTime", typeof(long));

            long changedUTCTime = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));

            foreach (var item in updated)
            {
                dt_updated.Rows.Add(item.SequenceNumber, item.CreatedUTCTime);
            }

            foreach (var sequenceNumber in inserted)
            {
                dt_Inserted.Rows.Add(sequenceNumber, changedUTCTime);
            }

            var isSuccessParameter = new SqlParameter("@IsSuccess", true) { Direction = ParameterDirection.Output };
            var updatedTable = new SqlParameter("@UpdatedTable", dt_updated) { SqlDbType = SqlDbType.Structured, TypeName = "SequenceNumberUTCTimeType" };
            var insertedTable = new SqlParameter("@InsertedTable", dt_Inserted) { SqlDbType = SqlDbType.Structured, TypeName = "SequenceNumberUTCTimeType" };

            SqlHelper2.DataTableToSqlServer(accountBuffer3Table);

            SqlHelper2.ExecuteNonQuery("spSeqActivateAccountList", updatedTable, insertedTable, new SqlParameter("@CreatedUTCTime", changedUTCTime), new SqlParameter("@BusinessUnit", BusinessUnit), new SqlParameter("@SessionId", sessionId), isSuccessParameter);
            isSuccess = Convert.ToBoolean(isSuccessParameter.Value);

            return isSuccess;
        }
    }
}
