using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Query
{
    public class IssueAndActiveCampaignVoucherInfo
    {
        public string AccountNumber { get; set; }
        public string ProgramCode { get; set; }
        public string MasterAccountNumber { get; set; }
        public string MasterAccountProgramCode { get; set; }
        public DateTime IssueDate { get; set; }
        public bool IsMaster { get; set; }
        public int BalanceInitialized { get; set; }
        public DateTime ActiveTo { get; set; }
    }
}
