using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecFlow.Test.Factory
{
    public class DataManager
    {
        private static Data data;

        public static Data GetData()
        {
            return data;
        }

        public static void SetData(Data datagen)
        {
            data = datagen;
        }

    }
}
