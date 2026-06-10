using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterSP
{
    public static class DataProvider
    {
        public static DataTable QueryProcessDetail(int ProcessLogId, out int result)
        {
            DataTable dt = new DataTable();
            using (SqlDataAdapter adapter = new SqlDataAdapter("spQueryProcessDetail", SqlHelper.MOConnectionString))
            {
                SqlParameter resultParam = new SqlParameter("@result", 0) { Direction = ParameterDirection.Output, SqlDbType = SqlDbType.Int };

                adapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                adapter.SelectCommand.Parameters.Add(new SqlParameter("@ProcessLogId", ProcessLogId));
                adapter.SelectCommand.Parameters.Add(resultParam);
                adapter.SelectCommand.CommandTimeout = 180;
                adapter.Fill(dt);
                result = Convert.ToInt32(resultParam.Value);
            }
            return dt;
        }
    }
}
