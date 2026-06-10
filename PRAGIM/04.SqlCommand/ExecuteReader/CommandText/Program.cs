using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandText
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Select * from Fitri_test with(nolock)";
                cmd.Connection = connection;
                connection.Open();
                var ds = cmd.ExecuteReader();
            }
        }
    }
}
