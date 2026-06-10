using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_Query1
{
    public class OrderDetailActionHistoryDataModel
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string ActionType { get; set; }
        public string ActionResult { get; set; }
        public string Operator { get; set; }
        public DateTime ActionTime { get; set; }
        public string EmailReceiver { get; set; }
    }
}
