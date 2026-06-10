using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteFromDiffDB
{
    public static class DataProvider
    {
        public static int GetMoveArcTranNumForTheDay(int Id, int DateNum)
        {
            int TransNumber = 0;
            var TransNumberParameter = new SqlParameter("@TransNumber", TransNumber) { Direction = ParameterDirection.Output };

            SqlParameter[] parameters = new SqlParameter[3] { new SqlParameter("@Id", Id), new SqlParameter("@DateNum", DateNum), TransNumberParameter };
            SqlHelper.ExecuteQuery("Adora", "spGetMoveTransNumForTheDay", parameters.ToArray());

            return (int)TransNumberParameter.Value;
        }
    }
}
