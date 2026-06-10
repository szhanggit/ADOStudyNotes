using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOAccess
{
    public static class SqlHelper
    {
        private static string _connectionStringName = "MODB";

        public static string MOConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            }
        }

        public static string MoveOlapConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MOVEOLAP"].ConnectionString;
            }
        }

        public static string TWMOVEARC202ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["TWMOVEARC202"].ConnectionString;
            }
        }

        public static string AdoraConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["Adora"].ConnectionString;
            }
        }

        public static void ExecuteNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString))
            {
                connection.Open();
                CreateSqlCommand(procedureName, parameters, connection).ExecuteNonQuery();
                connection.Close();
            }
        }

        public static DataSet ExecuteQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString))
            {
                var ds = new DataSet();
                connection.Open();
                var adapter = new SqlDataAdapter();
                adapter.SelectCommand = CreateSqlCommand(procedureName, parameters, connection);
                adapter.Fill(ds);
                return ds;
            }
        }

        public static DataSet ExecuteQuery(string connectionName, string procedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionName].ConnectionString))
            {
                var ds = new DataSet();
                connection.Open();
                var adapter = new SqlDataAdapter();
                adapter.SelectCommand = CreateSqlCommand(procedureName, parameters, connection);
                adapter.Fill(ds);
                return ds;
            }
        }

        public static T ExecuteScalar<T>(string procedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString))
            {
                connection.Open();
                var result = CreateSqlCommand(procedureName, parameters, connection).ExecuteScalar();
                return result == DBNull.Value || result == null ? default(T) : (T)result;
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
