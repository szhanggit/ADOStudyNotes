using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteReader
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Select * from Fitri_test with(nolock)", connection);
                connection.Open();
                var ds = cmd.ExecuteReader();
            }
        }
    }
}
