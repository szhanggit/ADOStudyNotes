using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query
{
    public class PreAuthorizationTranInfo
    {
        public string AccountNumber { get; set; }
        public int? TranBalanceAvailable { get; set; }
        public int? TranBalanceFrozen { get; set; }
        public string TranCode { get; set; }
        public string TranCodeRef { get; set; }
        public int RefundedAmount { get; set; }
        public short TranType { get; set; }
        public int TranAmount { get; set; }
        public byte TranStatus { get; set; }
        public string ResponseCode { get; set; }
        public string TranChecksum { get; set; }
        public DateTime TranUtcDateTime { get; set; }
        public bool isTranHistory { get; set; }

        public int ValueType { get; set; }
        public int BalanceAvailable { get; set; }
        public int BalanceFrozen { get; set; }
        public int AccountStatus { get; set; }
        public string AccountChecksum { get; set; }
        public int AccountModifyVersion { get; set; }

        public int ProgramId { get; set; }
        public string ProgramCode { get; set; }
        public string TimeOffset { get; set; }
        public int? RestrictionId { get; set; }
        public int PreAuthorizationExpiryUnit { get; set; }
        public int PreAuthorizationExpiryInterval { get; set; }
        public string SecurityValue { get; set; }
    }
}
