using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace CopyingData
{
    public class Process
    {
        public Process()
        {

        }

        public void Do()
        {
            string sourceCS = ConfigurationManager.ConnectionStrings["MODB"].ConnectionString;
            string destinationCS = ConfigurationManager.ConnectionStrings["Adora"].ConnectionString;
            using (SqlConnection sourceCon = new SqlConnection(sourceCS))
            {
                SqlCommand cmd = new SqlCommand("Select * from Departments2", sourceCon);
                sourceCon.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    using (SqlConnection destinationCon = new SqlConnection(destinationCS))
                    {
                        using (SqlBulkCopy bc = new SqlBulkCopy(destinationCon))
                        {
                            bc.DestinationTableName = "Departments2";

                            // As the column names in the source and destination tables
                            // are the same column mappings are not required.
                            // bc.ColumnMappings.Add("ID", "ID");
                            // bc.ColumnMappings.Add("Name", "Name"); 
                            // bc.ColumnMappings.Add("Location", "Location");

                            destinationCon.Open();
                            bc.WriteToServer(rdr);
                        }
                    }
                }

                cmd = new SqlCommand("Select * from Employees2", sourceCon);

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    using (SqlConnection destinationCon = new SqlConnection(destinationCS))
                    {
                        using (SqlBulkCopy bc = new SqlBulkCopy(destinationCon))
                        {
                            bc.DestinationTableName = "Employees2";
                            destinationCon.Open();
                            bc.WriteToServer(rdr);
                        }
                    }
                }
            }
        }
    }
}
