using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_ExecuteScalarSP
{
    public class PaymentEntity
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public decimal PaymentAmount { get; set; }
        public string ReferenceNumber { get; set; }
        public string PaidDate { get; set; }
    }
}
