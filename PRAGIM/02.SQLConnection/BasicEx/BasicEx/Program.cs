using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicEx
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection("data source=.; database=IN_EV_DBS_MOVE; integrated security=SSPI; uid=steven; pwd=steven;"))
            {
                SqlCommand cmd = new SqlCommand("Select * from Product with(nolock)", connection);
                connection.Open();
                var ds = cmd.ExecuteReader();
            }
        }
    }
}
