using Microsoft.WindowsAzure.Storage.Table;

namespace HobbyClue.Data.Azure.Base
{
    public abstract class BaseTableStorageRepository
    {
        protected readonly ICloudClientWrapper CloudClientWrapper;

        protected BaseTableStorageRepository(ICloudClientWrapper cloudClientWrapper)
        {
            this.CloudClientWrapper = cloudClientWrapper;
        }
        
        public void Delete(string tableName)
        {
            var table = CloudClientWrapper.TableClient.GetTableReference(tableName);
            table.DeleteIfExists();
        }

        public CloudTable GetOrCreate(string tableName)
        {
            var table = CloudClientWrapper.TableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            return table;
        }
    }
}
