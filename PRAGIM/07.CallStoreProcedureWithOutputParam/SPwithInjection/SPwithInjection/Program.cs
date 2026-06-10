using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SPwithInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            string txtEmployeeName = "Steven";
            string txtGender = "Male";
            string txtSalary = "23434534";

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("spAddEmployee", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                //Add the input parameters to the command object
                cmd.Parameters.AddWithValue("@Name", txtEmployeeName);
                cmd.Parameters.AddWithValue("@Gender", txtGender);
                cmd.Parameters.AddWithValue("@Salary", txtSalary);

                //Add the output parameter to the command object
                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = "@EmployeeId";
                outPutParameter.SqlDbType = System.Data.SqlDbType.Int;
                outPutParameter.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outPutParameter);

                //Open the connection and execute the query
                connection.Open();
                cmd.ExecuteNonQuery();

                string EmployeeId = outPutParameter.Value.ToString();
            }
        }
    }
}
