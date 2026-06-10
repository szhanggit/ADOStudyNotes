using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTableSelectEx
{
    class Program
    {
        /*https://www.cnblogs.com/xiaoxiaoshare/p/7197163.html*/
        static void Main(string[] args)
        {
            DataTable table = new DataTable();
            table.Columns.Add("Id", typeof(Int32));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("ProductCode", typeof(string));
            table.Columns.Add("Price", typeof(decimal));
            table.Columns.Add("ProductType", typeof(int));

            // Step 3: here we add rows.
            table.Rows.Add(12, "FriedChicken", "FC00001", 213.23, 1);
            table.Rows.Add(16, "Coak", "CK000001", 344.23, 2);
            table.Rows.Add(17, "Coak", "CK000002", 300.23, 2);
            table.Rows.Add(18, "Coak", "CK000003", 301.23, 2);
            table.Rows.Add(19, "Coak", "CK000004", 359.23, 2);
            table.Rows.Add(20, "Coak", "CK000005", 360.23, 2);
            table.Rows.Add(21, "Coak", "CK000006", 361.23, 2);
            table.Rows.Add(22, "Coak", "CK000007", 362.23, 2);
            table.Rows.Add(23, "Coak", "CK000008", 363.23, 2);

            DataRow[] drs = table.Select("Id > 20");
            DataRow[] drs2 = table.Select("ProductName = 'Coak'");
        }
    }
}
