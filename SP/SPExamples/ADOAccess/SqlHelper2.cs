using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Validation;

namespace ADOAccess
{
    public static class SqlHelper2
    {
        private static string _connectionStringName = "Adora";

        public static string AdoraConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            }
        }
        public static int Submit(this DbContext context)
        {
            try
            {
                return context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var errors = new StringBuilder();

                foreach (var result in ex.EntityValidationErrors)
                {
                    foreach (var err in result.ValidationErrors)
                    {
                        errors.Append(Helper.Append(err.PropertyName, ":", err.ErrorMessage, Environment.NewLine));
                    }
                }

                throw new EfException(errors.ToString(), ex);
            }
        }

        public static void DataTableToSqlServer(DataTable dataTable)
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

        public static DataTable QueryDataTable(string procedureName, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                var command = CreateSqlCommand(procedureName, parameters, connection);
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(table);
                connection.Close();
            }

            return table;
        }

        public static void ExecuteNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                CreateSqlCommand(procedureName, parameters, connection).ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void ExecuteNonQuery(string commandText)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                (new SqlCommand(commandText, connection)).ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void ExecuteLongNonQuery(string commandText)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand(commandText, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = int.MaxValue;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static T ExecuteScalar<T>(string commandText)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["d_ev_authorizationContext"].ConnectionString))
            {
                connection.Open();
                var ret = (new SqlCommand(commandText, connection)).ExecuteScalar();
                return (ret == DBNull.Value || ret == null) ? default(T) : (T)ret;
            }
        }


        private static SqlCommand CreateSqlCommand(string procedureName, SqlParameter[] parameters, SqlConnection connection)
        {
            var command = new SqlCommand(procedureName, connection);
            command.CommandType = CommandType.StoredProcedure;
            command.CommandTimeout = int.MaxValue;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }
    }
}
