using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsert1
{
    public static class DataProvider
    {
        public static void BulkCopyDataTable(DataTable shopWorkTable)
        {
            SqlHelper.DataTableToSqlServer(shopWorkTable);
        }
    }
}
