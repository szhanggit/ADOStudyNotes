using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkInsert0
{
    public class BatchTransactionTask
    {
        public BatchTransactionTask()
        {

        }

        public long Id
        {
            get;
            set;
        }

        public virtual byte Status
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> ExecuteStartTime
        {
            get;
            set;
        }

        public virtual Nullable<System.DateTime> ExecuteEndTime
        {
            get;
            set;
        }

        public virtual int TotalVoucherCount
        {
            get;
            set;
        }

        public virtual Nullable<int> SuccessVoucherCount
        {
            get;
            set;
        }

        public virtual Nullable<int> FailVoucherCount
        {
            get;
            set;
        }

        public virtual System.DateTime CreatedOn
        {
            get;
            set;
        }

        public virtual string CreatedBy
        {
            get;
            set;
        }

    }
}
