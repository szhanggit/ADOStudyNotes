using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReturnTwoTables
{
    public class SqlHelper
    {
        private static readonly Lazy<SqlHelper> instance = new Lazy<SqlHelper>(() => new SqlHelper(), LazyThreadSafetyMode.ExecutionAndPublication);
        private static string _connectionStringName = "MODB";

        private SqlHelper()
        {

        }

        static SqlHelper() { }

        public static SqlHelper Instance
        {
            get
            {
                return instance.Value;
            }
        }

        public string MOConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString;
            }
        }

        public string MoveOlapConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["MOVEOLAP"].ConnectionString;
            }
        }

        internal void ExecuteNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString))
            {
                connection.Open();
                CreateSqlCommand(procedureName, parameters, connection).ExecuteNonQuery();
            }
        }

        internal DataSet ExecuteQuery(string procedureName, params SqlParameter[] parameters)
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

        internal DataSet ExecuteQuery(string connectionName, string procedureName, params SqlParameter[] parameters)
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

        internal T ExecuteScalar<T>(string procedureName, params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings[_connectionStringName].ConnectionString))
            {
                connection.Open();
                var result = CreateSqlCommand(procedureName, parameters, connection).ExecuteScalar();
                return result == DBNull.Value || result == null ? default(T) : (T)result;
            }
        }

        private SqlCommand CreateSqlCommand(string procedureName, SqlParameter[] parameters, SqlConnection connection)
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
