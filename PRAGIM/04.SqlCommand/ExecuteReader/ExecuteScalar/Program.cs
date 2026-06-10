using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExecuteScalar
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select count(1) from Product with(nolock)", connection);
                connection.Open();
                int TotalRows = (int)cmd.ExecuteScalar();
            }
        }
    }
}
