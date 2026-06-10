using ADOAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetNextSeqVal
{
    public static class DataProvider
    {
        public static string GetIdentityCode()
        {
            return DataProvider.GetSequenceValue("Seq_Client_IdentityCode", true, 15, '0');
        }

        public static string GetSequenceValue(string SequenceDBName, bool isFixReturnLength, byte returnLength, char paddingCharacter)
        {
            string result = null;
            var ret = new SqlParameter("@ret", SqlDbType.VarChar, 100, ParameterDirection.Output, false, 0, 0, "ret", DataRowVersion.Current, result);
            SqlHelper.ExecuteNonQuery("dbo.getSequenceNextValue",
                new SqlParameter("@SequenceName", SequenceDBName),
                new SqlParameter("@IsFixReturnLength", isFixReturnLength),
                new SqlParameter("@ReturnLength", returnLength),
                new SqlParameter("@PaddingCharacter", paddingCharacter),
                ret);

            return ret.Value.ToString();
        }
    }
}
