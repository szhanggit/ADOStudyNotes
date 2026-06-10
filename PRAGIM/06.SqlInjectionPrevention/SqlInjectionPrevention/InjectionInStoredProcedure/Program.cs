using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionInStoredProcedure
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                string ProductNameText = "i'; Delete from tblProductInventory --";
                SqlCommand cmd = new SqlCommand("spGetProductsByName", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                // Associate the parameter and it's value with the command object
                cmd.Parameters.AddWithValue("@ProductName", ProductNameText + "%");
                connection.Open();
                var s = cmd.ExecuteReader();
            }
        }
    }
}
