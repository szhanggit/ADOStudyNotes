using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseSqlDataAdapter
{
    public class Process
    {
        private int _childProductId = 0;

        public Process()
        {

        }

        public void Do()
        {
            DataTable dt = DataProvider.GetComboInfoByMasterVoucher(1222482);
            foreach (DataRow dr in dt.Rows)
            {
                if (int.TryParse(dr["ChildProductId"].ToString(), out _childProductId))
                {

                }
            }
        }
    }
}
