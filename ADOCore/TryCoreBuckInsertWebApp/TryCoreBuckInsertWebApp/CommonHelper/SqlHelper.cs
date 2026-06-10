using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TryCoreBuckInsertWebApp.Models;

namespace TryCoreBuckInsertWebApp.CommonHelper
{
    public class SqlHelper : ISqlHelper
    {
        private readonly IConfiguration _config;

        public SqlHelper(IConfiguration config)
        {
            _config = config;
        }

        public string ConnectionString()
        {
            return _config.GetValue<string>("ConnectionString");
        }

        public void DataTableToSqlServer(DataTable dataTable)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionString(), SqlBulkCopyOptions.UseInternalTransaction))
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

        public void DataTableToSqlServer(DataTable dataTable, int timeout = 180)
        {
            if (dataTable != null && dataTable.Rows.Count > 0 && dataTable.Columns.Count > 0)
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(ConnectionString(), SqlBulkCopyOptions.UseInternalTransaction))
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
        public void ListToSqlServer(List<EmailMessage> emailMessageList)
        {
            string strSqlConnection = ConnectionString();
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
