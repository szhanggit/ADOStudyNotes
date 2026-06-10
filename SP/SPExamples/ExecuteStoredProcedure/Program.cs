using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            int clientId = 6;
            int dateFrom = 20190101;
            int dateTo = 20210501;
            bool isSuccess = DataProvider.CalculateInvoiceMandatory(clientId, dateFrom, dateTo);
        }
    }
}
