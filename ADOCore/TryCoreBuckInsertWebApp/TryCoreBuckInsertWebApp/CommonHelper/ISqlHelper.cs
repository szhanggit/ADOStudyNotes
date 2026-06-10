using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using TryCoreBuckInsertWebApp.Models;

namespace TryCoreBuckInsertWebApp.CommonHelper
{
    public interface ISqlHelper
    {
        string ConnectionString();
        void DataTableToSqlServer(DataTable dataTable);
        void DataTableToSqlServer(DataTable dataTable, int timeout = 180);
        void ListToSqlServer(List<EmailMessage> emailMessageList);

    }
}
