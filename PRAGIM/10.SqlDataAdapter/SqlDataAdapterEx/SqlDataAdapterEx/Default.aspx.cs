using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SqlDataAdapterEx
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                connection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from tblProductInventory with(nolock);", connection);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);

                GridView1.DataSource = dataset;
                GridView1.DataBind();
            }*/


            /*using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                connection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("spGetProductInventory", connection);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);

                GridView1.DataSource = dataset;
                GridView1.DataBind();
            }*/


            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                connection.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter("spGetProductInventoryById", connection);
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                dataAdapter.SelectCommand.Parameters.AddWithValue("@ProductId", 1);
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);

                GridView1.DataSource = dataset;
                GridView1.DataBind();
            }
        }
    }
}