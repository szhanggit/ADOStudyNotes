using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnTwoTables
{
    public class Process
    {
        public Process()
        {

        }

        public void Do()
        {
            List<DataTable> DTList = null;
            DataTable dt0 = new DataTable();
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable dt3 = new DataTable();
            DataTable dt4 = new DataTable();
            DataTable dt5 = new DataTable();

            List<string> AccountNumberList = new List<string> { "000000435413", "0003627456" };
            DTList = DataProvider.GetData("Adora", "spGetAdoraLECheckingData", AccountNumberList);
            if (DTList != null && DTList.Count > 0)
            {
                dt0 = DTList[0];
                dt1 = DTList[1];
                dt2 = DTList[2];
                dt3 = DTList[3];
                dt4 = DTList[4];
                dt5 = DTList[5];
            }
        }
    }
}
