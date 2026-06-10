using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_QueryFirstOrDefaultSP
{
    public class ClientOrderLine
    {
        public int Id { get; set; }
        public int ClientOrderId { get; set; }
        public int ClientQuotationProductId { get; set; }

        public string ProgramName { get; set; }
        public string ProgramId { get; set; }
        public string LocalRedemptionID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactPhone { get; set; }
        public string Country { get; set; }
        public string RedemptionDate { get; set; }
        public string RCN { get; set; }
        public string RedemptionItemName { get; set; }
        public string SKU { get; set; }
        public int Quantity { get; set; }
        public int? UnitItemPoints { get; set; }
        public int? TotalItemPoints { get; set; }
        //1:Email
        //2:SMS
        //3:File
        //4:Both
        public byte DeliveryType { get; set; }
        public string DeliveryPriority { get; set; }
        public string ClientOrderNumber { get; set; }
        public int Priority { get; set; }
        public string Remarks { get; set; }
        public string ClientSKUCode { get; set; }
        public string Flex1 { get; set; }
        public string Flex2 { get; set; }
        public string Flex3 { get; set; }
        public string Flex4 { get; set; }
        public string Flex5 { get; set; }
        //1:process
        //3:voucher get
        //4:Build TX Order
        //5:cancel
        public byte Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }

        public string FileName { get; set; }
        public int BatchId { get; set; }
        public int? BatchStatus { get; set; }
        public string OrderNumber { get; set; }
        public string Result { get; set; }
    }
}
