using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SqlDataReaderEx
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * from tblProductInventory2", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable sourceTable = new DataTable();
                    sourceTable.Columns.Add("ID");
                    sourceTable.Columns.Add("Name");
                    sourceTable.Columns.Add("Price");
                    sourceTable.Columns.Add("DiscountedPrice");

                    while (reader.Read())
                    {
                        //Calculate the 10% discounted price
                        int OriginalPrice = Convert.ToInt32(reader["UnitPrice"]);
                        double DiscountedPrice = OriginalPrice * 0.9;

                        // Populate datatable column values from the SqlDataReader
                        DataRow datarow = sourceTable.NewRow();
                        datarow["ID"] = reader["ProductId"];
                        datarow["Name"] = reader["ProductName"];
                        datarow["Price"] = OriginalPrice;
                        datarow["DiscountedPrice"] = DiscountedPrice;

                        //Add the DataRow to the DataTable
                        sourceTable.Rows.Add(datarow);
                    }
                }
            }
        }
    }
}
