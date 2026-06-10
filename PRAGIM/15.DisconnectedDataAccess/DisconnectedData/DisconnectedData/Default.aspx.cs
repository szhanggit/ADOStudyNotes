using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DisconnectedData
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void GetDataFromDB()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MODB"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            string selectQuery = "Select * from tblStudents";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Students");
            // Set ID column as the primary key
            dataSet.Tables["Students"].PrimaryKey = new DataColumn[] { dataSet.Tables["Students"].Columns["ID"] };
            // Store the dataset in Cache
            Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);

            GridView1.DataSource = dataSet;
            GridView1.DataBind();

            lblStatus.Text = "Data loded from Database";
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataSet dataSet = (DataSet)Cache["DATASET"];
            dataSet.Tables["Students"].Rows.Find(e.Keys["ID"]).Delete();
            Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
            GetDataFromCache();
        }

        private void GetDataFromCache()
        {
            if (Cache["DATASET"] != null)
            {
                GridView1.DataSource = (DataSet)Cache["DATASET"];
                GridView1.DataBind();
            }
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            // Set row in editing mode
            GridView1.EditIndex = e.NewEditIndex;
            GetDataFromCache();
        }

        protected void btnGetDataFromDB_Click(object sender, EventArgs e)
        {
            GetDataFromDB();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // Retrieve dataset from cache
            DataSet dataSet = (DataSet)Cache["DATASET"];
            // Find datarow to edit using primay key
            DataRow dataRow = dataSet.Tables["Students"].Rows.Find(e.Keys["ID"]);
            // Update datarow values
            dataRow["Name"] = e.NewValues["Name"];
            dataRow["Gender"] = e.NewValues["Gender"];
            dataRow["TotalMarks"] = e.NewValues["TotalMarks"];
            // Overwrite the dataset in cache
            Cache.Insert("DATASET", dataSet, null, DateTime.Now.AddHours(24), System.Web.Caching.Cache.NoSlidingExpiration);
            // Remove the row from edit mode
            GridView1.EditIndex = -1;
            // Reload data to gridview from cache
            GetDataFromCache();
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            GetDataFromCache();
        }

        protected void btnUpdateDatabaseTable_Click(object sender, EventArgs e)
        {
            if (Cache["DATASET"] != null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MODB"].ConnectionString;
                SqlConnection connection = new SqlConnection(connectionString);
                string selectQuery = "Select * from tblStudents";
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

                // Update command to update database table
                string strUpdateCommand = "Update tblStudents set Name = @Name, Gender = @Gender, TotalMarks = @TotalMarks where Id = @Id";
                // Create an instance of SqlCommand using the update command created above
                SqlCommand updateCommand = new SqlCommand(strUpdateCommand, connection);
                // Specify the parameters of the update command
                updateCommand.Parameters.Add("@Name", SqlDbType.NVarChar, 50, "Name");
                updateCommand.Parameters.Add("@Gender", SqlDbType.NVarChar, 20, "Gender");
                updateCommand.Parameters.Add("@TotalMarks", SqlDbType.Int, 0, "TotalMarks");
                updateCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                // Associate update command with SqlDataAdapter instance
                dataAdapter.UpdateCommand = updateCommand;

                // Delete command to delete data from database table
                string strDeleteCommand = "Delete from tblStudents where Id = @Id";
                // Create an instance of SqlCommand using the delete command created above
                SqlCommand deleteCommand = new SqlCommand(strDeleteCommand, connection);
                // Specify the parameters of the delete command
                deleteCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                // Associate delete command with SqlDataAdapter instance
                dataAdapter.DeleteCommand = deleteCommand;

                // Update the underlying database table
                dataAdapter.Update((DataSet)Cache["DATASET"], "Students");
                lblStatus.Text = "Database table updated";
            }
        }
    }
}