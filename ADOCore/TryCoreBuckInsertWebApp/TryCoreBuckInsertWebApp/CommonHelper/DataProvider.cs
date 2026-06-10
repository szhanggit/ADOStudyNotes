using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TryCoreBuckInsertWebApp.CommonHelper
{
    public class DataProvider : IDataProvider
    {
        private ISqlHelper _sqlHelper = null;
        public DataProvider(ISqlHelper sqlHelper)
        {
            this._sqlHelper = sqlHelper;
        }
        public void BulkCopyDataTable(DataTable shopWorkTable)
        {
            _sqlHelper.DataTableToSqlServer(shopWorkTable);
        }
    }
}
