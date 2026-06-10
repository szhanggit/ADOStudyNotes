using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionStringBuilderEx0
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Address> _list = DataProvider.GetPersonAddress();
        }
    }
}
