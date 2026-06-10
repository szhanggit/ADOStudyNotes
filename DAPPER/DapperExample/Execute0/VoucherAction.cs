using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Execute0
{
    public enum VoucherAction : short
    {
        Redemption = 101,
        ReverseRedemption = 201,
        PreAuthorization = 102,
        ReversePreAuthorization = 202,
        PreAuthorizationComplete = 103,
        ReversePreAuthorizationComplete = 203,
        AccountVerification = 104,
        BalanceEnquiry = 105,
        PosTest = 301,
        Issue = 401,
        ReverseIssue = 501,
        Activate = 402,
        ReverseActivate = 502,
        Expire = 403,
        ReverseExpire = 503,
        PreAuthorizationExpire = 106,
        Reload = 107,
        ReverseReload = 207,
        TrashMoney = 404,
        TrashNoMoney = 405,

        Block = 504,
        ReverseBlock = 505,
        UpdateOrderBeneficiaryInfo = 506,
        ResendEmailTask = 507,
        ResendSLMSTask = 508,
        SendEmail = 510,
        SendSlms = 511,

        SyncVoucherStatus = 512,
        Extend = 513,

        NotifyFormerUser = 514
    }
}
