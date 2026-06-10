using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TryCoreBuckInsertWebApp.Models
{
    public class EmailMessage
    {
        public long Id
        {
            get;
            set;
        }

        public long TaskId
        {
            get;
            set;
        }

        public Nullable<int> MessageStatus
        {
            get;
            set;
        }

        public long MessageLocalId
        {
            get;
            set;
        }

        public string MessageSPId
        {
            get;
            set;
        }

        public string To
        {
            get;
            set;
        }

        public string CC
        {
            get;
            set;
        }

        public string BCC
        {
            get;
            set;
        }

        public string Attachments
        {
            get;
            set;
        }

        public Nullable<System.DateTime> CreatedOn
        {
            get;
            set;
        }

        public Nullable<System.DateTime> SentOn
        {
            get;
            set;
        }

        public string Comment
        {
            get;
            set;
        }

        public string Parameters
        {
            get;
            set;
        }
    }
}
