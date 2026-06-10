using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DataSetEx
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter("spGetProductAndCategoriesData", connection);
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                DataSet dataset = new DataSet();
                dataAdapter.Fill(dataset);

                /*GridViewProducts.DataSource = dataset.Tables[0];
                GridViewProducts.DataBind();

                GridViewCategories.DataSource = dataset.Tables[1];
                GridViewCategories.DataBind();*/

                dataset.Tables[0].TableName = "Products";
                dataset.Tables[1].TableName = "Categories";

                GridViewProducts.DataSource = dataset.Tables["Products"];
                GridViewProducts.DataBind();

                GridViewCategories.DataSource = dataset.Tables["Categories"];
                GridViewCategories.DataBind();
            }
        }
    }
}