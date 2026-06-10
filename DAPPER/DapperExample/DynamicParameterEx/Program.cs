using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicParameterEx
{
    class Program
    {
        /*
         https://dapper-tutorial.net/parameter-dynamic
             */
        static void Main(string[] args)
        {
            List<string> ShopCodeList = new List<string> {
                "0000000001", "0000000004", "0000000005"
            };

            var shopNameList = DataProvider.GetShopName(ShopCodeList);
        }
    }
}
