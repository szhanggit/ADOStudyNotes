using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace BulkInsert1
{
    public static class SqlHelper
    {
        private static string _connectionStringName = "d_ev_authorizationContext";

        public static string AdoraConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            }
        }

        internal static void DataTableToSqlServer(DataTable dataTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
            {
                bulkCopy.BatchSize = 3000;
                bulkCopy.BulkCopyTimeout = 180;
                bulkCopy.DestinationTableName = dataTable.TableName;
                foreach (DataColumn column in dataTable.Columns)
                {
                    bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                }

                bulkCopy.WriteToServer(dataTable);
            }
        }

        public static void DataTableToSqlServer(DataTable dataTable, int timeout = 180)
        {
            if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Columns.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConfigurationManager.ConnectionStrings["MODB"].ConnectionString, SqlBulkCopyOptions.UseInternalTransaction))
                {
                    bulkCopy.DestinationTableName = dataTable.TableName;

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.BulkCopyTimeout = timeout;
                    bulkCopy.WriteToServer(dataTable);
                }
            }
        }



        /*
        if (emailMessageList.Count <= 20)
            new EmailMessageBo().SaveEmailMessageList(emailMessageList);
        else
            InsertEmailMessage.InsertEmailMessageList(emailMessageList);

        public void SaveEmailMessageList(IEnumerable<EmailMessage> messageList)
        {
            var emr = UnitOfWork.GetEmailMessageRepository();
            messageList.ToList().ForEach(m => emr.Create(m));
            UnitOfWork.Commit();
        }
             */
        public static void ListToSqlServer(List<EmailMessage> emailMessageList)
        {
            string strSqlConnection = ConfigurationManager.ConnectionStrings["MODB"].ToString();
            using (SqlConnection conn = new SqlConnection(strSqlConnection))
            {
                conn.Open();

                SqlTransaction sqlbulkTransaction = conn.BeginTransaction();
                //Call SqlTransaction If there are wrong when checking Constraints 
                SqlBulkCopy bcp = new SqlBulkCopy(conn, SqlBulkCopyOptions.CheckConstraints, sqlbulkTransaction);
                bcp.DestinationTableName = "EmailMessage";

                DataTable dt = CommonMethod.ConvertToDataSet(emailMessageList).Tables[0];
                dt.Columns.Remove("Task");
                dt.Columns.Remove("EmailMessageActionLogs");
                dt.Columns.Remove("OtherParameter");
                dt.Columns.Remove("Version");

                foreach (DataColumn dc in dt.Columns)
                {
                    bcp.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                }

                try
                {
                    bcp.WriteToServer(dt);
                    sqlbulkTransaction.Commit();
                }
                catch (Exception ex)
                {
                    sqlbulkTransaction.Rollback();
                    throw ex;
                }
                finally
                {
                    bcp.Close();
                    conn.Close();
                }
            }
        }
    }
}
