using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using DatabaseDeployer.Core.Services;
using DatabaseDeployer.Core.Services.Impl;
using ConnectionSettings = DatabaseDeployer.Core.Model.ConnectionSettings;

namespace DatabaseDeployer.Infrastructure.DatabaseManager.DataAccess
{

    public class QueryExecutor : IQueryExecutor
    {
        private readonly IConnectionStringGenerator _connectionStringGenerator;

        public QueryExecutor(IConnectionStringGenerator connectionStringGenerator)
        {
            _connectionStringGenerator = connectionStringGenerator;
        }

        public QueryExecutor()
            : this(new ConnectionStringGenerator())
        {

        }

        public void ExecuteNonQuery(ConnectionSettings settings, string sql, bool runAgainstSpecificDatabase)
        {
            string connectionString = _connectionStringGenerator.GetConnectionString(settings, runAgainstSpecificDatabase);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;

                    var scripts = SplitSqlStatements(sql);
                    foreach (var splitScript in scripts)
                    {
                        command.CommandText = splitScript;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public int ExecuteScalarInteger(ConnectionSettings settings, string sql)
        {
            string connectionString = _connectionStringGenerator.GetConnectionString(settings, true);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public string[] ReadFirstColumnAsStringArray(ConnectionSettings settings, string sql)
        {
            var list = new List<string>();
            string connectionString = _connectionStringGenerator.GetConnectionString(settings, true);

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = sql;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string item = reader[0].ToString();
                            list.Add(item);
                        }
                    }
                }


            }
            return list.ToArray();
        }

        private static IEnumerable<string> SplitSqlStatements(string sqlScript)
        {
            // Split by "GO" statements
            var statements = Regex.Split(
                    sqlScript,
                    @"^\s*GO\s* ($ | \-\- .*$)",
                    RegexOptions.Multiline |
                    RegexOptions.IgnorePatternWhitespace |
                    RegexOptions.IgnoreCase);

            // Remove empties, trim, and return
            return statements
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim(' ', '\r', '\n'));
        }
    }
}