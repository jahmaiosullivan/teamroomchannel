using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Dapper;

namespace HobbyClue.Data.Dapper
{
    public interface IQueryManager
    {
        IEnumerable<T> ExecuteQuery<T>(string queryName, object @params = null, IDictionary<string, string> replaceFields = null, CommandType commandType = CommandType.Text, int? commandTimeOut = default (int?));
        Task<IEnumerable<T>> ExecuteQueryAsync<T>(string queryName, object @params = null, IDictionary<string, string> replaceFields = null, CommandType commandType = CommandType.Text, int? commandTimeOut = default (int?));
        IEnumerable<T> ExecuteSql<T>(string sql, object @params = null, IDictionary<string, string> replaceFields = null, CommandType? commandType = null, int? commandTimeOut = default (int?));
        T ExecuteScalar<T>(string sql, object @params, int? commandTimeOut = default (int?));

        IEnumerable<TReturn> ExecuteSql<TFirst, TSecond, TReturn>(string sql, Func<TFirst, TSecond, TReturn> map, object @params = null, int? commandTimeOut = default (int?), string splitOn = "Id");
        Task<IEnumerable<T>> ExecuteSqlAsync<T>(string sql, object @params = null, IDictionary<string, string> replaceFields = null, CommandType? commandType = null, int? commandTimeOut = default (int?));

        IEnumerable<TReturn> ExecuteQuery<TFirst, TSecond, TReturn>(string queryName, Func<TFirst, TSecond, TReturn> map, object @params = null, int? commandTimeOut = default (int?), string splitOn = "Id");
        IEnumerable<dynamic> ExecuteDynamic(string queryName, object @params = null, CommandType commandType = CommandType.Text);
        void ExecuteMultiple(string queryName, Action<SqlMapper.GridReader> readAction, object @params = null, CommandType commandType = CommandType.Text);
        int ExecuteProc(string procName, object @params = null);
        int ExecuteNonQuery(string sql, object @params);
        int ExecuteNonQuery(IDbCommand command);
        string GetQuery(string queryName);

        T ExecuteScalar<T>(DbCommand cmd);
        Task<T> ExecuteScalarAsync<T>(DbCommand cmd);
    }
}