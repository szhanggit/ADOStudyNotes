using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOAccess
{
    public static class CommonUtility
    {
        private static ConcurrentDictionary<string, IDictionary<string, object>> voucherStockInfoDictionaryWithProgramCode = new ConcurrentDictionary<string, IDictionary<string, object>>();//缓存
        private static ConcurrentDictionary<int, IDictionary<string, object>> voucherStockInfoDictionary = new ConcurrentDictionary<int, IDictionary<string, object>>();//缓存

        public static IDictionary<string, object> GetVoucherStockInfoWithProgramCode(string ProgramCode)
        {
            if (voucherStockInfoDictionaryWithProgramCode.ContainsKey(ProgramCode))
            {
                IDictionary<string, object> value;
                voucherStockInfoDictionaryWithProgramCode.TryGetValue(ProgramCode, out value);
                return value;
            }

            IDictionary<string, object> dic;
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                dic = conn.MO_Query(SqlStringManager.QueryStockInfoWithProgramCode, new { ProgramCode = ProgramCode }).FirstOrDefault() as IDictionary<string, object>;
            }
            if (dic != null)
            {
                voucherStockInfoDictionaryWithProgramCode.TryAdd(ProgramCode, dic);
            }
            return dic;
        }

        public static string GetVoucherStockTableNameWithProgramCode(string ProgramCode)
        {
            IDictionary<string, object> dic = GetVoucherStockInfoWithProgramCode(ProgramCode);
            if (dic != null)
            {
                return dic["VoucherStockTableName"].ToString();
            }
            return null;
        }

        public static string GetVoucherStockPrefix(int programId)
        {
            IDictionary<string, object> dic = GetVoucherStockInfo(programId);
            if (dic != null)
            {
                return dic["ShortUrlPrefix"].ToString();
            }
            return null;
        }

        public static string GetVoucherStockTableName(int programId)
        {
            IDictionary<string, object> dic = GetVoucherStockInfo(programId);
            if (dic != null)
            {
                return dic["VoucherStockTableName"].ToString();
            }
            return null;
        }

        private static IDictionary<string, object> GetVoucherStockInfo(int programId)
        {
            if (voucherStockInfoDictionary.ContainsKey(programId))
            {
                IDictionary<string, object> value;
                voucherStockInfoDictionary.TryGetValue(programId, out value);
                return value;
            }

            IDictionary<string, object> dic;
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                dic = conn.MO_Query(SqlStringManager.QueryStockInfo, new { programId = programId }).FirstOrDefault() as IDictionary<string, object>;
            }
            if (dic != null)
            {
                voucherStockInfoDictionary.TryAdd(programId, dic);
            }
            return dic;
        }
    }
}
