using ADOAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteNonQueryText
{
    public static class DataProvider
    {
        public static void RemoveAccountBuffer1(string taskId)
        {
            SqlHelper2.ExecuteNonQuery(string.Format("DELETE FROM dbo.AccountBuffer1 WHERE TaskId = '{0}'", taskId));
        }
    }
}
