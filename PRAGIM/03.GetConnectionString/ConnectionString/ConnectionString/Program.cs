using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionString
{
    class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = ConfigurationManager.ConnectionStrings["MODB"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("Select * from Product with(nolock)", connection);
                connection.Open();
                var dat = cmd.ExecuteReader();
            }
        }
    }
}
