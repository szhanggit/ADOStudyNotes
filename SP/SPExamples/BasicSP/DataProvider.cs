using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSP
{
    public static class DataProvider
    {
        public static void InsertOlapDBTransaction()
        {
            string result = null;
            int line = 0;
            var errorMessage = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 4000, ParameterDirection.Output, false, 0, 0, "ErrorMessage", DataRowVersion.Current, result);
            var errorLine = new SqlParameter("@ErrorLine", SqlDbType.Int, int.MaxValue, ParameterDirection.Output, false, 0, 0, "ErrorLine", DataRowVersion.Current, line);

            using (var connection = new SqlConnection(SqlHelper.MoveOlapConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "spInsertOLAPTransaction";
                    cmd.CommandTimeout = 30 * 60;//30 mins
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(new SqlParameter[] { errorMessage, errorLine });
                    cmd.ExecuteNonQuery();
                }
            }

            if (!string.IsNullOrEmpty(errorMessage.Value.ToString()))
            {
                throw new InvalidOperationException(string.Format("Message: {0}; Error Line: {1}", errorMessage.Value, errorLine.Value ?? 0));
            }
        }
    }
}
