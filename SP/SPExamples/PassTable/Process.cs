using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassTable
{
    public class Process
    {
        public Process()
        {

        }

        private void Processing(List<ProductApplyOrderLog> list, int start, int end, DataTable SkippedList)
        {
            int index = 0;
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("RecordId", typeof(int));
            table.Columns.Add("BatchLogId", typeof(int));
            table.Columns.Add("ProductCode", typeof(string));
            table.Columns.Add("OrderNumber", typeof(string));
            table.Columns.Add("IsEmail", typeof(int));

            foreach (ProductApplyOrderLog item in list.GetRange(start, end))
            {
                table.Rows.Add(++index, item.Id, item.TaskId, item.ProductCode, item.OrderNumber, item.IsEmail);
            }

            DataProvider.ProcessProductEmailSLMSTemplateApplyOrder(table, SkippedList);
        }
    }
}
