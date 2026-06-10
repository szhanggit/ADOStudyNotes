using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteStoredProcedure
{
    public static class DataProvider
    {
        public static bool CalculateInvoiceMandatory(int clientId, int dateFrom, int dateTo)
        {
            return ExecuteStoredProcedure("spCalculateInvoiceMandatory", new SqlParameter("@DateFrom", dateFrom), new SqlParameter("@DateTo", dateTo), new SqlParameter("@ClientId", clientId));
        }

        private static bool ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            bool isSuccess = false;
            var isSuccessParameter = new SqlParameter("@IsSuccess", isSuccess) { Direction = ParameterDirection.Output };

            var sqlParameters = new List<SqlParameter> { isSuccessParameter };

            if (parameters != null)
            {
                sqlParameters.AddRange(parameters);
            }

            SqlHelper.ExecuteNonQuery(procedureName, sqlParameters.ToArray());

            return (bool)isSuccessParameter.Value;
        }
    }
}
