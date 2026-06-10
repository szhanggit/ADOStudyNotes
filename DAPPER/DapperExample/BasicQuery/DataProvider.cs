using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicQuery
{
    public static class DataProvider
    {
        public static Voucher GetVoucherInfoByAlias(string ShortUrl)
        {
            using (SqlConnection conn = new SqlConnection(SqlHelper.MOConnectionString))
            {
                var sql = @"select * from Voucher with(nolock) where ShortUrl = @ShortUrl";
                return conn.MO_QueryFirstOrDefault<Voucher>(sql, new { @ShortUrl = ShortUrl });
            }
        }
    }
}
