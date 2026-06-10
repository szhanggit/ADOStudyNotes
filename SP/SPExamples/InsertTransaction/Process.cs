using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsertTransaction
{
    public class Process
    {
        public Process()
        {

        }

        public void DoInsertTransaction()
        {
            try
            {
                DataProvider.InsertTransaction();             
            }
            catch (Exception ex)
            {

            }
        }
    }
}
