using ADOAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execute0
{
    public static class DataProvider
    {
        public static void AddCSSReceiverOperation(long obiId, string voucherNumber, VoucherAction actionType, ActionResult actionResult, string operators, string memo)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                if (!string.IsNullOrEmpty(memo))
                {
                    if (memo.Length > 500)
                    {
                        memo = memo.Substring(0, 500);
                    }
                }
                string sql = $@"INSERT dbo.CSSReceiverOperation
                                (
                                    BeneficiaryInfoId,
                                    VoucherNumber,
                                    ActionType,
                                    ActionResult,
                                    Operator,
                                    ActionTime,
                                    Memo
                                )
                                VALUES
                                (   {obiId},
                                    @voucherNumber,
                                    {(int)actionType},
                                    {(int)actionResult},
                                    @operators,
                                    GETDATE(),
                                    @memo
                                    )";
                conn.Execute(sql, new { voucherNumber, operators, memo });
            }
        }
    }
}
