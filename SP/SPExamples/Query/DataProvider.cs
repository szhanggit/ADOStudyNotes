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

namespace Query
{
    public static class DataProvider
    {
        public static IEnumerable<Transaction> FetchFromTransactionExpTemp(string SessionId, int RowCount)
        {
            IEnumerable<Transaction> affectedRows;
            string ErrorMessage = string.Empty;
            string ErrorLine = string.Empty;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                var p = new DynamicParameters();
                p.Add("@SessionId", SessionId);
                p.Add("@RowCount", RowCount);
                p.Add("@ErrorMessage", ErrorMessage, dbType: DbType.StringFixedLength, size: 500, direction: ParameterDirection.Output);
                p.Add("@ErrorLine", ErrorLine, dbType: DbType.StringFixedLength, size: 500, direction: ParameterDirection.Output);

                affectedRows = connection.Query<Transaction>(sql: "spFetchFromTransactionExpTemp", param: p, commandType: CommandType.StoredProcedure);
                connection.Close();
            }

            return affectedRows;
        }

        public static IEnumerable<IssueAndActiveCampaignVoucherInfo> FetchIssueAndActiveCampaignVoucherInfo(string sessionId, int count)
        {
            IEnumerable<IssueAndActiveCampaignVoucherInfo> affectedRows;
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                var p = new DynamicParameters();
                p.Add("@SessionId", sessionId);
                p.Add("@Count", count);

                affectedRows = connection.Query<IssueAndActiveCampaignVoucherInfo>(sql: "spFetchIssueAndActiveCampaignVoucherInfo", param: p, commandType: CommandType.StoredProcedure);
                connection.Close();
            }

            return affectedRows;
        }

        public static List<PreAuthorizationTranInfo> GetPreAuthorizationExpire()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                return connection.Query<PreAuthorizationTranInfo>("spGetPreAuthorizationExpire", commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
