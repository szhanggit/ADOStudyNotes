using ADOAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicParameterEx
{
    public static class DataProvider
    {
        public static List<string> GetShopName(List<string> ShopCodeList)
        {
            string sql = @"select [Name] from Shop with(nolock) where IdentityCode in ({0})";
            try
            {
                var parameters = new DynamicParameters();
                string[] ShopCodes = ShopCodeList.Select((s, v) => "@ShopCode" + v.ToString()).ToArray();

                for (int s = 0; s < ShopCodes.Length; s++)
                {
                    parameters.Add(ShopCodes[s], ShopCodeList[s]);
                }

                sql = string.Format(sql, string.Join(",", ShopCodes));
                using (var con = new SqlConnection(SqlHelper.MOConnectionString))
                {
                    return con.Query<string>(sql, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
