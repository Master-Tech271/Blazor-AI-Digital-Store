using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLORM
{
    public class SQLData
    {
        private static string _connectionString = string.Empty;
        private const int _commandTimeout = 120;

        public static string DataConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                {
                    _connectionString = Convert.ToString(Environment.GetEnvironmentVariable("DataConnectionString")) ?? string.Empty;
                }
                return _connectionString;
            }
        }

        public static IDbConnection GetConnection()
        {
            return new SqlConnection(DataConnectionString);
        }

        public static IEnumerable<T> Query<T>(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Query<T>(sql, parameters, commandTimeout: _commandTimeout);
            }
        }

        public static T QuerySingleOrDefault<T>(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return connection.QuerySingleOrDefault<T>(sql, parameters, commandTimeout: _commandTimeout);
            }
        }

        public static int Execute(string sql, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Execute(sql, parameters, commandTimeout: _commandTimeout);
            }
        }

        public static IEnumerable<T> ExecuteQuery<T>(string storedProcedure, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Query<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }

        public static int ExecuteNonQuery(string storedProcedure, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return connection.Execute(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }

        public static T ExecuteScalar<T>(string storedProcedure, object parameters = null)
        {
            using (var connection = GetConnection())
            {
                return connection.ExecuteScalar<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
            }
        }
    }
}
