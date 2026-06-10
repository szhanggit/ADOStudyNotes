using Dapper;
using System.Collections.Generic;
using System.Data;

namespace ADOAccess
{
    public static class DapperHelper
    {
        private const int defaultTimeout = 180;//默认超时时间，单位为秒
        public static int MiddleTimeout = 600;
        public static int LongTimeout = 1800;

        public static IEnumerable<dynamic> MO_Query(this IDbConnection conn, string sql, object param = null, int commandTimeout = defaultTimeout, CommandType commandType = CommandType.Text)
        {
            return conn.Query(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public static IEnumerable<T> MO_Query<T>(this IDbConnection conn, string sql, object param = null, int commandTimeout = defaultTimeout, CommandType commandType = CommandType.Text)
        {
            return conn.Query<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public static T MO_QueryFirstOrDefault<T>(this IDbConnection conn, string sql, object param = null, int commandTimeout = defaultTimeout, CommandType commandType = CommandType.Text)
        {
            return conn.QueryFirstOrDefault<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public static int MO_Execute(this IDbConnection conn, string sql, object param = null, int commandTimeout = defaultTimeout, CommandType commandType = CommandType.Text)
        {
            return conn.Execute(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }

        public static T MO_ExecuteScalar<T>(this IDbConnection conn, string sql, object param = null, int commandTimeout = defaultTimeout, CommandType commandType = CommandType.Text)
        {
            return conn.ExecuteScalar<T>(sql, param, commandTimeout: commandTimeout, commandType: commandType);
        }
    }
}
