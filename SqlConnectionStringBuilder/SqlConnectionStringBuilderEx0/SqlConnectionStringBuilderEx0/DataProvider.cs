using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnectionStringBuilderEx0
{
    public static class DataProvider
    {
        public static CustomConnectionFactory ccf = new CustomConnectionFactory();
        public static List<Address> GetPersonAddress()
        {
            //using (SqlConnection conn = new SqlConnection(SqlHelper.AdventureWorksDB))
            using (SqlConnection conn = (SqlConnection)ccf.CreateConnection("adsf"))
            {
                return conn.MO_Query<Address>("select top(100) * from [Person].[Address] with(nolock)").ToList();
            }
        }
    }
}
