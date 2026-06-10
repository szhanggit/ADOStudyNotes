using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_ExecuteScalar0
{
    public static class DataProvider
    {
        public static bool ActivePreissuedVouchersV2(long beneficiaryInfoId, Dictionary<long, string> authCodeDic)
        {
            StringBuilder sb = new StringBuilder(256);
            sb.AppendLine("SET NOCOUNT ON;");
            sb.AppendLine("BEGIN TRY");
            sb.AppendLine("BEGIN TRAN");
            foreach (var item in authCodeDic)
            {
                sb.AppendLine($"UPDATE dbo.Voucher SET BeneficiaryInfoId = {beneficiaryInfoId}, Status = 2, AuthCode = '{item.Value}' WHERE Id = {item.Key};");
            }
            sb.AppendLine("    COMMIT");
            sb.AppendLine("    SELECT 1");
            sb.AppendLine("END TRY");
            sb.AppendLine("BEGIN CATCH");
            sb.AppendLine("    ROLLBACK");
            sb.AppendLine("    SELECT 0");
            sb.AppendLine("END CATCH");

            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                return conn.MO_ExecuteScalar<int>(sb.ToString()) == 1;
            }
        }


    }
}
