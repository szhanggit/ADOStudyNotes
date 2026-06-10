using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query
{
    public class Transaction
    {
        public Transaction()
        {
            Status = 1;
        }
        public string AccountNumber { get; set; }
        public string ProgramCode { get; set; }
        public string MerchantCode { get; set; }
        public string HostSSN { get; set; }
        public string TerminalSSN { get; set; }
        public int TranUtcDate { get; set; }
        public long TranUtcDateTimeL { get; set; }
        public System.DateTime TranUtcDateTime { get; set; }
        public Nullable<int> TranLocalDate { get; set; }
        public Nullable<System.DateTime> TranLocalDateTime { get; set; }
        public Nullable<System.DateTime> TranTerminalDateTime { get; set; }
        public Nullable<int> SettleDate { get; set; }
        public System.DateTime TranRealDateTime { get; set; }
        public string ShopCode { get; set; }
        public string TerminalCode { get; set; }
        public short TranType { get; set; }
        public byte Status { get; set; }
        public int TranAmount { get; set; }
        public int RefundedAmount { get; set; }
        public string ResponseCode { get; set; }
        public Nullable<bool> IsCleared { get; set; }
        public Nullable<int> NodeNum { get; set; }
        public string CallerIP { get; set; }
        public string Channel { get; set; }
        public string Parameters { get; set; }
        public string TranCode { get; set; }
        public string TranCodeRef { get; set; }
        public System.Guid TranGUID { get; set; }
        public Nullable<System.Guid> TranGUIDRef { get; set; }
        public Nullable<int> ModifyVersion { get; set; }
        public Nullable<int> BalanceAvailable { get; set; }
        public Nullable<int> BalanceFrozen { get; set; }
        public string Checksum { get; set; }
        public Nullable<int> SecurityKeyId { get; set; }
        public string Rsv1 { get; set; }
        public string Rsv2 { get; set; }
        public string Rsv3 { get; set; }

        public string ShiftCode { get; set; }
        public int? BusinessDay { get; set; }
        //[NotMapped]
        public int? PreTranType { get; set; }

        public int? InitBalance { get; set; }

        public string SessionId { get; set; }

        public int MultiAccountsTransactionId { get; set; }

        public int LEBalance { get; set; }

        public int LEInitBalance { get; set; }
    }
}
