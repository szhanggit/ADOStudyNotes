using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_Query1
{
    public class PreIssuedCampaignVoucherRequestInfo
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public long? OrderBeneficiaryInfoId { get; set; }
        public string ProductCode { get; set; }
        public int VoucherQuantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public int ProcessStatus { get; set; }
    }
}
