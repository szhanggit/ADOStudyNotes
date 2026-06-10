using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataTable
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable result = DataProvider.GetAutoCalculateReimbursementMerchant();
            if (result != null)
            {
                foreach (DataRow row in result.Rows)
                {
                    int merchantId = int.Parse(row["Id"].ToString());
                }
            }
        }
    }
}
