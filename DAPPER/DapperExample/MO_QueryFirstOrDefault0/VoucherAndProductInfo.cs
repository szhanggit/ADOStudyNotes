using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MO_QueryFirstOrDefault0
{
    public class VoucherAndProductInfo
    {
        public string CurrentProgramIdentityCode { get; set; }
        public string CurrentVoucherNumber { get; set; }
        public long CurrentVoucherId { get; set; }
        public int VoucherStatus { get; set; }
        public string ShortURL { get; set; }
        public int ProgramId { get; set; }
        public string ProductName { get; set; }
        public int VoucherNumberGenerateWay { get; set; }
        public int VoucherComboid { get; set; }
        public string MasterRedemptionTranCode { get; set; }
        public int? MasterRedemptionTranAmount { get; set; }


        public long ChildVoucherId { get; set; }
        public string ChildAccountProgramIdentityCode { get; set; }
        public string ChildAccountNumber { get; set; }
        public int ChildAccountStatus { get; set; }
        public int ChildAccountBalanceInitialed { get; set; }
        public int ChildAccountBalanceAvailable { get; set; }


        public long MasterVoucherId { get; set; }
        public string MasterAccountProgramIdentityCode { get; set; }
        public string MasterAccountNumber { get; set; }
        public int MasterAccountStatus { get; set; }
        public int MasterAccountBalanceInitialed { get; set; }
        public int MasterAccountBalanceAvailable { get; set; }

        public int ProductType { get; set; }
    }
}
