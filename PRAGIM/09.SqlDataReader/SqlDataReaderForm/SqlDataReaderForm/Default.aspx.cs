using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SqlDataReaderForm
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from tblProductInventory3 with(nolock); select * from tblProductCategories3 with(nolock)", connection);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    ProductsGridView.DataSource = reader;
                    ProductsGridView.DataBind();

                    while (reader.NextResult())
                    {
                        CategoriesGridView.DataSource = reader;
                        CategoriesGridView.DataBind();
                    }
                }
            }
        }
    }
}