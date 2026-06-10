using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsert0
{
    public class BatchTransactionDetail
    {
        public BatchTransactionDetail()
        {

        }

        private long _batchTransactionId;

        public long Id
        {
            get;
            set;
        }

        public virtual string ProgramCode
        {
            get;
            set;
        }

        public virtual string VoucherNumber
        {
            get;
            set;
        }

        public virtual short TranType
        {
            get;
            set;
        }

        public virtual Nullable<int> Amount
        {
            get;
            set;
        }

        public virtual string MerchantCode
        {
            get;
            set;
        }

        public virtual string ShopCode
        {
            get;
            set;
        }

        public virtual string TerminalCode
        {
            get;
            set;
        }

        public virtual Nullable<int> SettleDate
        {
            get;
            set;
        }

        public virtual string Reason
        {
            get;
            set;
        }

        public virtual string ResponseCode
        {
            get;
            set;
        }

        public virtual string Comment
        {
            get;
            set;
        }

        public virtual string TranCode
        {
            get;
            set;
        }

        public virtual string TranCodeRef
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> TranRealDateTime
        {
            get;
            set;
        }

        public virtual string Rsv1
        {
            get;
            set;
        }

        public virtual string Rsv2
        {
            get;
            set;
        }

        public virtual string Rsv3
        {
            get;
            set;
        }

        private Nullable<long> _batchTransactionTaskId;

        public virtual Nullable<System.DateTime> ExpiryDate
        {
            get;
            set;
        }

        public virtual string VoucherGuid
        {
            get;
            set;
        }

        public virtual Nullable<byte> MGCType
        {
            get;
            set;
        }

        public virtual string ChildVouchers
        {
            get;
            set;
        }

    }
}
