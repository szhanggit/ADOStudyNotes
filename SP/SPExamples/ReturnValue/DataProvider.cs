using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReturnValue
{
    public static class DataProvider
    {
        public static bool ActivePreissuedVouchers(string voucherIds, long beneficiaryInfoId, int voucherStatus)
        {
            var result = new SqlParameter("@resultParameter", SqlDbType.Int) { Direction = ParameterDirection.ReturnValue };
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@VoucherIds", voucherIds),
                new SqlParameter("@BeneficiaryInfoId", beneficiaryInfoId),
                new SqlParameter("@Status", voucherStatus),
                result
            };

            SqlHelper.ExecuteNonQuery("spActivePreissuedVouchers", parameters.ToArray());

            return (int)result.Value > 0;

        }
    }
}
