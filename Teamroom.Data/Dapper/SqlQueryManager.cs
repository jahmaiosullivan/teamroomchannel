using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Dapper;
using HobbyClue.Common.Models;

namespace HobbyClue.Data.Dapper
{
    public class SqlQueryManager : IQueryManager
    {
        private static readonly Func<Assembly> _assemblyProvider;
        private static readonly Func<string> _resourceNamePrefixProvider;
        private static readonly ConcurrentDictionary<string, string> _queryCache = new ConcurrentDictionary<string, string>();
        private static readonly Func<string[]> _resourceNamesProvider;

        private static DbConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        static SqlQueryManager()
        {
            _assemblyProvider = () => typeof(SqlQueryManager).Assembly;
            _resourceNamePrefixProvider = () => "Teamroom.Data.SqlQueries.";
            _resourceNamesProvider = () => _assemblyProvider().GetManifestResourceNames();
        }

        public IEnumerable<T> ExecuteQuery<T>(string queryName, object @params = null, IDictionary<string, string> replaceFields = null, CommandType commandType = CommandType.Text, int? commandTimeOut = default (int?))
        {
            var query = commandType == CommandType.Text ? GetQuery(queryName) : queryName;
            return ExecuteSql<T>(query, @params, replaceFields, commandType, commandTimeOut);
        }
        
        public IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TReturn>(string queryName, Func<TFirst, TSecond, TReturn> map, object @params = null, int? commandTimeOut = default (int?), string splitOn = "Id")
        {
            var sql = GetQuery(queryName);
            return ExecuteSql(sql, map, @params, commandTimeOut, splitOn);
        }

        public async Task<IEnumerable<T>> ExecuteQueryAsync<T>(string queryName, object @params = null, IDictionary<string, string> replaceFields = null, CommandType commandType = CommandType.Text, int? commandTimeOut = default (int?))
        {
            var query = commandType == CommandType.Text ? GetQuery(queryName) : queryName;
            return await ExecuteSqlAsync<T>(query, @params, replaceFields, commandType, commandTimeOut);
        }
        
        public IEnumerable<T> ExecuteSql<T>(string sql, object @params = null, IDictionary<string, string> replaceFields = null, CommandType? commandType = null, int? commandTimeOut = default (int?))
        {
            ReplaceFields(ref sql, replaceFields);
            commandType = commandType ?? CommandType.Text;

            using (var connection = GetConnection())
            {
                var results = commandTimeOut == default(int?)
                    ? connection.Query<T>(sql, commandType: commandType, param: @params)
                    : connection.Query<T>(sql, commandType: commandType, param: @params, commandTimeout: commandTimeOut);
                return results.EmptyListIfNull();
            }
        }

        public IEnumerable<TReturn> ExecuteSql<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object @params = null, int? commandTimeOut = default (int?), string splitOn = "Id")
        {
            const CommandType commandType = CommandType.Text;
            using (var connection = GetConnection())
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                return (commandTimeOut == default(int?) ?
                    connection.Query(sql, map, param: @params, commandType: commandType, splitOn: splitOn) :
                    connection.Query(sql, map, commandType: commandType, param: @params, commandTimeout: commandTimeOut, splitOn: splitOn));
            }
        }
        
        public async Task<IEnumerable<T>> ExecuteSqlAsync<T>(string sql, object @params = null, IDictionary<string, string> replaceFields = null, CommandType? commandType = null, int? commandTimeOut = default (int?))
        {
            ReplaceFields(ref sql, replaceFields);
            commandType = commandType ?? CommandType.Text;

            using (var connection = GetConnection())
            {
                var results = commandTimeOut == default(int?)
                    ? await connection.QueryAsync<T>(sql, commandType: commandType, param: @params)
                    : await connection.QueryAsync<T>(sql, commandType: commandType, param: @params, commandTimeout: commandTimeOut);
                return results;
            }
        }
        
        public IEnumerable<dynamic> ExecuteDynamic(string queryName, object @params = null, CommandType commandType = CommandType.Text)
        {
            var query = commandType == CommandType.Text ? GetQuery(queryName) : queryName;
            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.Query(query, commandType: commandType, param: @params);
            }
        }

        public void ExecuteMultiple(string queryName, Action<SqlMapper.GridReader> readAction, object @params = null, CommandType commandType = CommandType.Text)
        {
            var query = commandType == CommandType.Text ? GetQuery(queryName) : queryName;
            using (var connection = GetConnection())
            {
                connection.Open();
                var multipleResults = connection.QueryMultiple(query, commandType: commandType, param: @params);
                readAction(multipleResults);
            }
        }
        
        public int ExecuteProc(string procName, object @params = null)
        {
            if (procName == null)
                throw new ArgumentNullException("procName",
                                                "Expected a non-null string containing the stored procedure name to execute.");
            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.Execute(procName, commandType: CommandType.StoredProcedure, param: @params);
            }
        }

        public int ExecuteNonQuery(string sql, object @params)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new ArgumentNullException("sql",
                                                "Expected a non-null string containing the stored procedure name to execute.");

            using (var connection = GetConnection())
            {
                connection.Open();
                return connection.Execute(sql, @params);
            }
        }


        public int ExecuteNonQuery(IDbCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.CommandText))
                throw new ArgumentNullException("CommandText", "Expected a non-null string containing the stored procedure name to execute.");

            using (var connection = GetConnection())
            {
                connection.Open();
                command.Connection = connection;
                return command.ExecuteNonQuery();
            }
        }
        

      
        public string GetQuery(string queryName)
        {
            if (string.IsNullOrEmpty(queryName))
                throw new ArgumentNullException("queryName", "Expected a non-null value string containing the query name");
            
            string query;
            var resourceName = _resourceNamePrefixProvider() + queryName + ".sql";
            if (!_queryCache.TryGetValue(resourceName, out query))
            {
                if (_resourceNamesProvider().Any(x => x.Equals(resourceName, StringComparison.InvariantCulture)))
                {
                    using (var stream = _assemblyProvider().GetManifestResourceStream(resourceName))
                        if (stream != null)
                            using (var reader = new StreamReader(stream))
                            {
                                query = reader.ReadToEnd();
                                reader.Close();
                                stream.Close();
                            }
                }
            }

            return query;
        }

        public virtual T ExecuteScalar<T>(DbCommand cmd)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                cmd.Connection = conn;
                return (T)cmd.ExecuteScalar();
            }
        }

        public virtual T ExecuteScalar<T>(string sql, object @params, int? commandTimeOut = default (int?))
        {
            using (var connection = GetConnection())
            {
                var results = connection.Query<T>(sql, @params, commandTimeout: commandTimeOut).FirstOrDefault();
                return results;
            }
        }

        public async virtual Task<T> ExecuteScalarAsync<T>(DbCommand cmd)
        {
            using (var conn = GetConnection())
            {
                cmd.Connection = conn;
                conn.Open();
                return (T)await cmd.ExecuteScalarAsync();
            }
        }

        private static void ReplaceFields(ref string sql, IDictionary<string, string> replaceFields)
        {
            if (replaceFields == null || !replaceFields.Any()) return;
            foreach (var field in replaceFields)
            {
                sql = sql.Replace(string.Format("##{0}##", field.Key), field.Value);
            }
        }
    }

}