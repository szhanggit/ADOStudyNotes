using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteNonQuery2
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public int ProgramId { get; set; }
        public int ProductId { get; set; }
        public string ExternalOrderId { get; set; }
        public string AccountNumberHashed { get; set; }
        public int BalanceAvailable { get; set; }
        public int BalanceFrozen { get; set; }
        public int ValueType { get; set; }
        public int TopupMode { get; set; }
        public int Status { get; set; }
        public string Checksum { get; set; }
        public int TranCount { get; set; }
        public int TranAmountAccumulated { get; set; }
        public int AcceptanceLoopId { get; set; }
        public Nullable<int> RestrictionId { get; set; }
        public Nullable<int> ExpirationPolicyId { get; set; }
        public string Password { get; set; }
        public System.DateTime ActiveFrom { get; set; }
        public System.DateTime ActiveTo { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> LastTranOn { get; set; }
        public Nullable<int> LastTranDate { get; set; }
        public Nullable<long> LastTranTime { get; set; }
        public Nullable<System.DateTime> BlockedOn { get; set; }
        public Nullable<System.DateTime> ActivatedOn { get; set; }
        public Nullable<System.DateTime> ExpiredOn { get; set; }
        public int ModifyVersion { get; set; }
        public byte AccountNumberLength { get; set; }
        public Nullable<System.Guid> AccountGUID { get; set; }
        public bool IsNotifyClient { get; set; }
        public string ClientNotificationProviderCode { get; set; }
        public Nullable<System.DateTime> ConsumeTime { get; set; }
        public string ConsumeMerchantCode { get; set; }
        public string ConsumeShopCode { get; set; }

        public string ConsumeTerminalSSN { get; set; }
        public int BalanceInitialized { get; set; }
        public string PinCode { get; set; }
        public string MasterAccountNumber { get; set; }
        public string MasterProgramIdentityCode { get; set; }
        public int? MasterRedemptionTranAmount { get; set; }
        public long? SequenceNumber { get; set; }
    }
}
