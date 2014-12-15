using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HobbyClue.Common.Helpers;

namespace HobbyClue.Data.Dapper
{
    public interface IDapperRepository<T> : IRepository<T> where T : class
    {
        T GetById<TValType>(TValType idvalue);
        T Get<TValType>(string column, TValType idvalue);
        bool ItemExists(T item);
    }


    public abstract class BaseDapperRepository<T> : IDapperRepository<T> where T : class, new()
    {
        protected readonly IQueryManager _queryManager;

        protected BaseDapperRepository(IQueryManager queryManager)
        {
            _queryManager = queryManager;
        }
        
        public virtual T Insert(T item)
        {
            var cmd = CreateInsertCommand(item);
            var id = _queryManager.ExecuteScalar<object>(cmd);
            item.SetPrimaryKey(id);
            return item;
        }

        public async virtual Task<T> InsertAsync(T item)
        {
            var cmd = CreateInsertCommand(item);
            var id = await _queryManager.ExecuteScalarAsync<string>(cmd);
            item.SetPrimaryKey(id);
            return item;
        }
        
        public virtual bool Update(T item)
        {
            var command = CreateUpdateCommand(item);
            var response = _queryManager.ExecuteNonQuery(command);
            return Convert.ToBoolean(response);
        }

        public virtual bool Remove(T item)
        {
            var primaryKey = item.GetPrimaryKeyField();
            var deleteSql = string.Format("DELETE FROM [{1}] where {0} = @FieldValue", primaryKey.Name, TableName);
            _queryManager.ExecuteSql<int>(deleteSql, new { @FieldValue = primaryKey.Value });
            return true;
        }
        
        public virtual IEnumerable<T> Find(string query = null)
        {
            return _queryManager.ExecuteSql<T>(query ?? BaseQuery);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(string query = null)
        {
            return await _queryManager.ExecuteSqlAsync<T>(query ?? BaseQuery);
        }

        public virtual string BaseQuery
        {
            get
            {
                var result = _queryManager.GetQuery(string.Format("Get{0}", TableName));
                if (string.IsNullOrEmpty(result))
                    result = "select t1.* from " + TableName + " t1 ";
                return result; 
            }
        }
        
        public virtual string TableName
        {
            get
            {
                var entityName = Regex.Replace(GetType().Name, "Repository$", "");
                if (entityName.EndsWith("y"))
                    entityName = entityName.Substring(0, entityName.Length - 1) + "ies";
                else
                    entityName += "s";
                return entityName;
            }
        }

        #region "Helper functions"
        private DbCommand CreateInsertCommand(T insertingItem)
        {
            var settings = (IDictionary<string, object>)insertingItem.ToDynamic(true);
            var sbKeys = new StringBuilder();
            var sbVals = new StringBuilder();

            //var shouldIgnoreId = typeof(T).GetProperty("Id").GetCustomAttributes(false).Any(x => x is DapperIgnoreOnSaveOrUpdateAttribute);
            var primaryKey = insertingItem.GetPrimaryKeyField();

            var stub = "INSERT INTO {0} ({1}) \r\n OUTPUT INSERTED." + primaryKey.Name + " \r\n VALUES ({2})";
            var result = (SqlCommand)CreateCommand(stub, null);
            var counter = 0;
            foreach (var item in settings)
            {
                sbKeys.AppendFormat("{0},", item.Key);
                sbVals.AppendFormat("@{0},", counter);
                result.AddParam<T>(item.Value);
                counter++;
            }
            if (counter > 0)
            {
                var keys = sbKeys.ToString().Substring(0, sbKeys.Length - 1);
                var vals = sbVals.ToString().Substring(0, sbVals.Length - 1);
                var sql = string.Format(stub, TableName, keys, vals);
                result.CommandText = sql;
            }
            else throw new InvalidOperationException("Can't parse this object to the database - there are no properties set");
            return result;
        }

        private IDbCommand CreateCommand(string sql, IDbConnection conn, params object[] args)
        {
            IDbCommand result = new SqlCommand();
            result.Connection = conn;
            result.CommandText = sql;
            if (args.Length > 0)
                result.AddParams<T>(args);
            return result;
        }

        private IDbCommand CreateUpdateCommand(T updatedItem)
        {
            var settings = (IDictionary<string, object>)updatedItem.ToDynamic();
            var sbKeys = new StringBuilder();
            const string stub = "UPDATE {0} SET {1} WHERE {2} = @{3}";
            var result = CreateCommand(stub, null);
            var counter = 0;
            var primaryKey = updatedItem.GetPrimaryKeyField();
            foreach (var item in settings)
            {
                var val = item.Value;
                if (!item.Key.Equals(primaryKey.Name, StringComparison.OrdinalIgnoreCase) && item.Value != null)
                {
                    result.AddParam<T>(val);
                    sbKeys.AppendFormat("{0} = @{1}, \r\n", item.Key, counter);
                    counter++;
                }
            }
            if (counter > 0)
            {
                //add the key
                result.AddParam<T>(primaryKey.Value);
                //strip the last commas
                var keys = sbKeys.ToString().Substring(0, sbKeys.Length - 4);
                result.CommandText = string.Format(stub, TableName, keys, primaryKey.Name, counter);
            }
            else throw new InvalidOperationException("No parsable object was sent in - could not divine any name/value pairs");
            return result;
        }

        private IDbCommand CreateDeleteCommand(string primaryKeyFieldName, object keyValue = null, string where = "", params object[] args)
        {
            var sql = string.Format("DELETE FROM {0} ", TableName);
            if (keyValue != null)
            {
                sql += string.Format("WHERE {0}=@0", primaryKeyFieldName);
                args = new[] { keyValue };
            }
            else if (!string.IsNullOrEmpty(where))
            {
                sql += where.Trim().StartsWith("where", StringComparison.OrdinalIgnoreCase) ? where : "WHERE " + where;
            }
            return CreateCommand(sql, null, args);
        }

        #endregion

        public virtual T GetById<TValType>(TValType idvalue)
        {
            var item = new T();
            var primaryKeyField = item.GetPrimaryKeyField();
            return Get(primaryKeyField.Name, (TValType)DapperExtensions.ChangeType(idvalue.ToString(), primaryKeyField.Type));
        }

        public T Get<TValType>(string column, TValType val)
        {
            var sql = string.Format(BaseQuery + " where {0} = @Val", column);
            var results = _queryManager.ExecuteSql<T>(sql, new { @Val = val });
            return results.FirstOrDefault();
        }
        
        public virtual bool ItemExists(T item)
        {
            var primaryKeyField = item.GetPrimaryKeyField();
            return GetById(primaryKeyField.Value) != null;
        }
    }
}
