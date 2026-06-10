using ADOAccess;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_ExecuteSP
{
    public static class DataProvider
    {
        public static void SyncResendVoucher(int batchId)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                conn.MO_Execute(@"spSyncResendVoucher", new { batchId }, commandType: CommandType.StoredProcedure);
            }
        }

        public static void UpdateReservationBatchAvailableDate(List<string> SourceBatchNumberList, List<string> DestBatchNumberList, string AvailableStartDate, string AvailableEndDate, DateTime ExpiryDate)
        {
            if (SourceBatchNumberList == null || SourceBatchNumberList.Count == 0 || DestBatchNumberList == null || DestBatchNumberList.Count == 0) return;

            string SourceBatchNumbers = "";
            foreach (string BatchNumber in SourceBatchNumberList)
                SourceBatchNumbers += BatchNumber + "|";
            SourceBatchNumbers.TrimEnd('|');

            string DestBatchNumbers = "";
            foreach (string BatchNumber in DestBatchNumberList)
                DestBatchNumbers += BatchNumber + "|";
            DestBatchNumbers.TrimEnd('|');

            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                conn.MO_Execute("spUpdateReservationBatchAvailableDate", new { @SourceBatchNumbers = SourceBatchNumbers, @DestBatchNumbers = DestBatchNumbers, @AvailableStartDate = AvailableStartDate, @AvailableEndDate = AvailableEndDate, @ExpiryDate = ExpiryDate }, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
