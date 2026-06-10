using ADOAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteNonQuery0
{
    public static class DataProvider
    {
        public static void UpdateLEFromBuffer()
        {
            SqlHelper2.ExecuteLongNonQuery("spUpdateLEFromBuffer");
        }

        public static void UpdateAccountLETransStatus()
        {
            SqlHelper2.ExecuteLongNonQuery("spUpdateAccountLETransStatus");
        }

        public static void ProcessOldLE()
        {
            SqlHelper2.ExecuteLongNonQuery("spProcessOldLE");
        }
    }
}
