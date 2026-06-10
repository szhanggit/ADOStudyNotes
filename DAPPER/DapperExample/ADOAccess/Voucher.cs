using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOAccess
{
    public class Voucher
    {
        private Nullable<long> _beneficiaryInfoId;

        public virtual int BalanceAvailable
        {
            get;
            set;
        }

        public virtual int BalanceFrozen
        {
            get;
            set;
        }

        public virtual int Status
        {
            get;
            set;
        }

        public virtual Nullable<byte> DistributionEmailStatus
        {
            get;
            set;
        }

        public virtual Nullable<byte> DistributionSLMSStatus
        {
            get;
            set;
        }

        public virtual string DistributionSLMSMemo
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> PublishedOn
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> ActivatedOn
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> ExpiryDate
        {
            get;
            set;
        }

        public virtual string Memo
        {
            get;
            set;
        }

        public virtual string DistributionEmailMemo
        {
            get;
            set;
        }

        public virtual string EmailServiceProviderCode
        {
            get;
            set;
        }

        public virtual string SLMSServiceProviderCode
        {
            get;
            set;
        }

        public virtual System.Guid GUID
        {
            get;
            set;
        }


        private Nullable<long> _reservationBatchId;

        public virtual Nullable<System.DateTime> ConsumeTime
        {
            get;
            set;
        }

        public virtual string ConsumeMerchantCode
        {
            get;
            set;
        }

        public virtual string ConsumeShopCode
        {
            get;
            set;
        }

        public virtual Nullable<byte> CacheNode
        {
            get;
            set;
        }

        public virtual string ConsumeTerminalSSN
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> LastTranOn
        {
            get;
            set;
        }

        public virtual string TrashReason
        {
            get;
            set;
        }

        public virtual string ShortUrl
        {
            get;
            set;
        }

        public virtual string AuthCode
        {
            get;
            set;
        }

        public virtual Nullable<long> ExtendId
        {
            get;
            set;
        }

        public virtual string VSessionId
        {
            get;
            set;
        }

        public virtual string PinCode
        {
            get;
            set;
        }
    }
}
