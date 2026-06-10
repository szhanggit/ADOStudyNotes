using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace TryCoreBuckInsertWebApp.CommonHelper
{
    public interface IDataProvider
    {
        void BulkCopyDataTable(DataTable shopWorkTable);
    }
}
