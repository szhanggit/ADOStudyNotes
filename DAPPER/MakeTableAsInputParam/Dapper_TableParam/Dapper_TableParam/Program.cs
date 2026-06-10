using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Dapper_TableParam.Model;
using Microsoft.SqlServer.Server;

namespace Dapper_TableParam
{
    class Program
    {
        private static List<Customer_Input> SelectedCustomer = null;
        private static List<Customer_Output> CompanyList = null;
        private static string _connection = "NorthwindDB";
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings[_connection].ConnectionString;
        static void Main(string[] args)
        {
            var TypeSelectedCustomerParameter = new List<SqlDataRecord>();
            var myMetaData = new SqlMetaData[] {
                    new SqlMetaData("CustomerId", SqlDbType.VarChar,50),
                    new SqlMetaData("ContactName", SqlDbType.VarChar,50)
                };

            SelectedCustomer = (new Customer_Input()).GetData();

            foreach (var sc in SelectedCustomer)
            {
                var record = new SqlDataRecord(myMetaData);
                record.SetValue(0, sc.CustomerId);
                record.SetValue(1, sc.ContactName);
                TypeSelectedCustomerParameter.Add(record);
            }

            CompanyList = QueryIn(TypeSelectedCustomerParameter);
        }

        public static List<Customer_Output> QueryIn(List<SqlDataRecord> TypeSelectedCustomerParameter)
        {
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                return connection.MO_Query<Customer_Output>("spGetCompaniesOfCustomers", new { @TableName = "Customers", @Customers = TypeSelectedCustomerParameter.AsTableValuedParameter("Customer_Input") }, commandType: CommandType.StoredProcedure).ToList();
            }
        }
    }
}
