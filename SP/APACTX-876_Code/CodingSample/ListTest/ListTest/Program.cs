using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int step = 15;


            int startPoint = 0;                       
            List<string> list = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17" };
            int fixedLength = list.Count();
            int currentLength = 0;
            int length = list.Count() / step;
            int remind = list.Count() % step;

            if (remind > 0)
            {
                length++;
            }

            for (int i = 0; i < length; i++)
            {
                startPoint = step * i;
                currentLength = step * (i + 1);
                if (currentLength < fixedLength)
                {
                    subRoutine(list, startPoint, step);
                }
                else
                {
                    subRoutine(list, startPoint, (fixedLength - (currentLength - step)));
                }

                Console.WriteLine("-----------------------------------------------");
            }



            Console.ReadKey();
        }

        private static void subRoutine(List<string> list, int start, int end)
        {
            foreach (string item in list.GetRange(start, end))
            {
                Console.WriteLine(item);
            }
        }
    }
}
