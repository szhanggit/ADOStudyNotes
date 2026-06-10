using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InjectionAttack
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                string ProductNameText = "i'; Delete from tblProductInventory --";
                SqlCommand cmd = new SqlCommand("Select * from tblProductInventory where ProductName like '" + ProductNameText + "%'", connection);
                connection.Open();
                var s = cmd.ExecuteReader();
            }
        }
    }
}
