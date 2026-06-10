using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplelyQuerySP
{
    public class Process
    {
        private string _voucherNumber;
        private int _programId;

        public Process()
        {

        }

        public void Do()
        {
            DataRow voucherDR = DataProvider.GetVoucherInfoByAliasSP("EE9mfn8pv5");
            if (voucherDR != null)
            {
                _voucherNumber = voucherDR["VoucherNumber"].ToString();
                if (int.TryParse(voucherDR["ProgramId"].ToString(), out _programId))
                {

                }
            }
        }
    }
}
