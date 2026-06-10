using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_Query0
{
    public class QueryMerchantAndShopResult
    {
        public int Total { get; set; }
        public string MerchantCode { get; set; }
        public string MerchantName { get; set; }
        public string ShopCode { get; set; }
        public string ShopName { get; set; }
        public int Status { get; set; }
        public string ProgramCode { get; set; }
    }
}
