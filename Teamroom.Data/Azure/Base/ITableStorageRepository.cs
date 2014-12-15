using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace HobbyClue.Data.Azure.Base
{
    public interface ITableStorageRepository<T> : IRepository<T> where T : TableEntity, new()
    {
        T Find(string partitionkey, string rowkey);
        IEnumerable<T> Get(string rowKey, string partitionKey);
        CloudTable Table { get; }
        void Delete(string tableName);
        CloudTable GetOrCreate(string tableName);
        T GetById(string partition, string primaryKey);
    }
}