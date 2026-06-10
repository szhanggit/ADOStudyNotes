using ADOAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execute0
{
    public static class DataProvider
    {
        public static int InsertedExpiredAccounts(string SessionId)
        {
            string ErrorMessage = string.Empty;
            string ErrorLine = string.Empty;
            int isSuccess = -1;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                var p = new DynamicParameters();
                p.Add("@SessionId", SessionId);
                p.Add("@ErrorMessage", ErrorMessage, dbType: DbType.StringFixedLength, size: 500, direction: ParameterDirection.Output);
                p.Add("@ErrorLine", ErrorLine, dbType: DbType.StringFixedLength, size: 500, direction: ParameterDirection.Output);
                p.Add("@IsSuccess", isSuccess, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute(sql: "spInsertedExpiredAccounts", param: p, commandType: CommandType.StoredProcedure);
                isSuccess = p.Get<int>("@IsSuccess");
                ErrorMessage = p.Get<string>("@ErrorMessage");
                ErrorLine = p.Get<string>("@ErrorLine");
                connection.Close();
            }

            return isSuccess;
        }
    }
}
