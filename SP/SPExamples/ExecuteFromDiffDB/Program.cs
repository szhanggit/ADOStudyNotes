using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteFromDiffDB
{
    class Program
    {
        static void Main(string[] args)
        {
            int Id = 9;
            int DateNum = 20200101;
            int MoveArcTranNum = DataProvider.GetMoveArcTranNumForTheDay(Id, DateNum);
        }
    }
}
