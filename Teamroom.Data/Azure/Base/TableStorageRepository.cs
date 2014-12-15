using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HobbyClue.Common.Extensions;
using Microsoft.WindowsAzure.Storage.Table;

namespace HobbyClue.Data.Azure.Base
{
    public abstract class TableStorageRepository<T> : BaseTableStorageRepository, ITableStorageRepository<T> where T : TableEntity, new()
    {
        protected TableStorageRepository(ICloudClientWrapper cloudClientWrapper)
            : base(cloudClientWrapper)
        {
        }

        public T Find(string partitionkey, string rowkey)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(partitionkey, rowkey);
            var retrievedResult = Table.Execute(retrieveOperation);
            return (T)retrievedResult.Result;
        }

        public IEnumerable<T> Get(string rowKey, string partitionKey)
        {
            var rangeQuery = new TableQuery<T>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));
            return Table.ExecuteQuery(rangeQuery);
        }

        public T Insert(T item)
        {
            var insertOperation = TableOperation.Insert(item);
            Table.Execute(insertOperation);
            return item;
        }

        public Task<T> InsertAsync(T item)
        {
            throw new NotImplementedException();
        }

        public bool Update(T item)
        {
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(item);
            Table.Execute(insertOrReplaceOperation);
            return true;
        }
        
        public void Insert(IEnumerable<T> items)
        {
            var entities = (IList<TableEntity>) items.ToList();
            if (entities.Count > 100)
                throw new Exception("Can only insert up to 100 items");

            var batchOperation = new TableBatchOperation();
            foreach (var entity in entities)
            {
                batchOperation.Insert(entity);
            }
            Table.ExecuteBatch(batchOperation);
        }

        public bool Remove(T item)
        {
            var retrieveOperation = TableOperation.Retrieve(item.PartitionKey, item.RowKey);            var retrievedResult = Table.Execute(retrieveOperation);            var deleteEntity = (DynamicTableEntity)retrievedResult.Result;
            if (deleteEntity != null)
            {
                var deleteOperation = TableOperation.Delete(deleteEntity);
                Table.Execute(deleteOperation);
            }
            return true;
        }
        
        public IEnumerable<T> Find(string partitionkey = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var rangeQuery = GetBaseQuery(null);

            if(startDate.HasValue && endDate.HasValue)
                rangeQuery = rangeQuery.Where(
                   TableQuery.CombineFilters(
                       TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, startDate.Value.PaddedTicks()),
                       TableOperators.And,
                       TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, endDate.Value.PaddedTicks())));
            else if(startDate.HasValue)
                rangeQuery = rangeQuery.Where(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, startDate.Value.PaddedTicks()));
            else if (endDate.HasValue)
                rangeQuery = rangeQuery.Where(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, endDate.Value.PaddedTicks()));

            return Table.ExecuteQuery(rangeQuery);
        }

        public IEnumerable<T> Find(string query)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> FindAsync(string query = null)
        {
            throw new NotImplementedException();
        }

        private static TableQuery<T> GetBaseQuery(string partitionKey)
        {
            var query = new TableQuery<T>();
            if (partitionKey != null)
                query = query.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            return query;
        }

        public T GetById(string partition, string primaryKey)
        {
            return Find(partition, primaryKey);
        }

        public abstract string TableName { get; }
        public CloudTable Table
        {
            get { return GetOrCreate(TableName); }
        }

    }
}