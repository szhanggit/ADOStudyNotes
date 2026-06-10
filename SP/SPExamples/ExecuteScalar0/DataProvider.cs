using ADOAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteScalar0
{
    public static class DataProvider
    {
        public static string GetProgramCode(int programId)
        {
            return SqlHelper2.ExecuteScalar<string>(string.Format("SELECT p.IdentityCode from Program p with(nolock) where p.Id = {0}", programId));
        }

        public static bool IsMasterAccount(string accountNumber, int programId)
        {
            return SqlHelper2.ExecuteScalar<bool>(string.Format("select P.IsMasterProduct from Account A with(nolock) inner join Product P with(nolock) on A.ProductId = P.Id where A.AccountNumber = '{0}' and A.ProgramId = {1} ", accountNumber, programId));
        }
    }
}
