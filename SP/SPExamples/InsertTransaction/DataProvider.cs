using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertTransaction
{
    public static class DataProvider
    {
        public static void InsertTransaction()
        {
            ExecuteStoredProcedureWithDetail("spInsertTransaction");
        }

        public static void TrustAccountReport(byte reportType)
        {
            ExecuteStoredProcedureWithDetail("spTrustAccountReport", new SqlParameter("@ReportType", reportType));
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

        public static string MoveVoucherInStock(int MovingAmount, string SourceProductCode, string DestinationProductCode,
        string ExpiryDate, string BatchStartDate, string BatchEndDate, string SExpiryDate, string SAvailableStartDate, string SAvailableEndDate)
        {
            string result = string.Empty;
            var errorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000, ParameterDirection.Output, false, 0, 0, "ErrorMessage", DataRowVersion.Current, result);
            var ResultPara = new SqlParameter("@ResponseCode", SqlDbType.NVarChar, 50, ParameterDirection.Output, false, 0, 0, "ResponseCode", DataRowVersion.Current, result);
            List<SqlParameter> parameters =
              new List<SqlParameter>()
                {
                    new SqlParameter("@SourceProductCode", SourceProductCode),
                    new SqlParameter("@DestinationProductCode", DestinationProductCode),
                    new SqlParameter("@SAvailableStartDate", SAvailableStartDate),
                    new SqlParameter("@SAvailableEndDate", SAvailableEndDate),
                    new SqlParameter("@RequiredAmount", MovingAmount),
                    new SqlParameter("@AvailableStartDate", BatchStartDate),
                    new SqlParameter("@AvailableEndDate", BatchEndDate),
                    ResultPara,
                    errorMessage
                  };

            if (!string.IsNullOrEmpty(SExpiryDate))
            {
                parameters.Add(new SqlParameter("@SExpiryDate", SExpiryDate));
            }

            if (!string.IsNullOrEmpty(ExpiryDate))
            {
                parameters.Add(new SqlParameter("@ExpiryDate", ExpiryDate));
            }

            SqlHelper.ExecuteQuery("spVoucherStockMoving", parameters.ToArray());
            if (!string.IsNullOrEmpty(errorMessage.Value.ToString()))
            {
                throw new InvalidOperationException(string.Format("Message: {0};", errorMessage.Value));
            }
            return ResultPara.Value.ToString();
        }
    }
}
