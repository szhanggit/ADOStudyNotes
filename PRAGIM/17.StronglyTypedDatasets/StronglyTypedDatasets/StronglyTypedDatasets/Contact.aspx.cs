using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using StronglyTypedDatasets.Model;

namespace StronglyTypedDatasets
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                StudentDataSetTableAdapters.StudentsTableAdapter studentsTableAdapter =
                    new StudentDataSetTableAdapters.StudentsTableAdapter();

                StudentDataSet.StudentsDataTable studentsDataTable =
                    new StudentDataSet.StudentsDataTable();
                studentsTableAdapter.Fill(studentsDataTable);

                Session["DATATABLE"] = studentsDataTable;

                GridView1.DataSource = from student in studentsDataTable
                                       select new { student.ID, student.Name, student.Gender, student.TotalMarks };
                GridView1.DataBind();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            StudentDataSet.StudentsDataTable studentsDataTable =
            (StudentDataSet.StudentsDataTable)Session["DATATABLE"];

            if (string.IsNullOrEmpty(TextBox1.Text))
            {
                GridView1.DataSource = from student in studentsDataTable
                                       select new { student.ID, student.Name, student.Gender, student.TotalMarks };
                GridView1.DataBind();
            }
            else
            {
                GridView1.DataSource = from student in studentsDataTable
                                       where student.Name.ToUpper().StartsWith(TextBox1.Text.ToUpper())
                                       select new { student.ID, student.Name, student.Gender, student.TotalMarks };
                GridView1.DataBind();
            }
        }
    }
}