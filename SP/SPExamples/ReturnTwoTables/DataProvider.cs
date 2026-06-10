using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnTwoTables
{
    public static class DataProvider
    {
        public static List<DataTable> GetData(string connection, string sp, List<string> AccountNumberList)
        {
            List<DataTable> DTList = new List<DataTable>();

            DataTable AccountNumberTable = new DataTable();
            AccountNumberTable.Columns.Add("StrElement", typeof(string));

            foreach (string item in AccountNumberList)
            {
                AccountNumberTable.Rows.Add(item);
            }

            SqlParameter[] parameters = new SqlParameter[1] { new SqlParameter("@AccountNumberList", AccountNumberTable) };
            parameters[0].SqlDbType = SqlDbType.Structured;
            parameters[0].TypeName = "SingleStringListType";

            DataSet result = SqlHelper.Instance.ExecuteQuery(connection, sp, parameters);
            if (result.Tables.Count > 0)
            {
                foreach (DataTable dt in result.Tables)
                {
                    DTList.Add(dt);
                }

                return DTList;
            }
            else
            {
                return null;
            }
        }
    }
}
