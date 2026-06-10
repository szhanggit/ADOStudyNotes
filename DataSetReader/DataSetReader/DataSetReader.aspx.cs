using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace DataSetReader
{
    public partial class DataSetReader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            String CnStr = ConfigurationManager.ConnectionStrings["DBConnection"].ToString();
            SqlConnection cn = new SqlConnection(CnStr);
            SqlDataAdapter da = new SqlDataAdapter("select student_classid, studentid, classid from student_class", cn);
            DataSet ds = new DataSet();
            da.Fill(ds, "table1");
            DataTable Dt = ds.Tables["table1"];
            DataTableReader Dtr = Dt.CreateDataReader();

            while(Dtr.Read())
            {
                String student_classid = Dtr["student_classid"].ToString();
                String studentid = Dtr["studentid"].ToString();
                String classid = Dtr["classid"].ToString();
            }

            Dtr.Close();
        }
    }
}