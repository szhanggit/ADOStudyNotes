using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_Execute0
{
    class Program
    {
        static void Main(string[] args)
        {
            DiveTask dt = new DiveTask();
            dt.Id = 1;
            dt.Status = 2;
            dt.ExecuteStartTime = new DateTime(2018, 12, 21, 11, 48, 12);
            DataProvider.UpdateDiveTask(dt);
        }
    }
}
