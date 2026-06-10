using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCommand2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("insert into Fitri_test (test) values ('1111');", connection);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("update Fitri_test set test = '0000' where test = '1111';", connection);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from Fitri_test;", connection);
                connection.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
            }
        }
    }
}
