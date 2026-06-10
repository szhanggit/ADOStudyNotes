using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_Execute0
{
    public class DiveTask
    {
        public long Id { get; set; }
        public int OrderId { get; set; }
        public int OrderLineId { get; set; }
        public int StartOrderLineSN { get; set; }
        public int EndOrderLineSN { get; set; }
        public byte Status { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExecuteStartTime { get; set; }
        public DateTime ExecuteEndTime { get; set; }
        public string SessionId { get; set; }
        /// <summary>
        /// OrderLine 是否分多次处理
        /// </summary>
        public bool IsPartialPublish { get; set; }
        public object manager { get; set; }
        //public IList<Voucher> voucherList { get; set; }
    }
}
