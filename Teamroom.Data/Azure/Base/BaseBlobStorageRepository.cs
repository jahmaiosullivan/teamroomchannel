using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HobbyClue.Data.Azure.Base
{
    public abstract class BaseBlobStorageRepository
    {
        protected readonly ICloudClientWrapper cloudClientWrapper;

        protected BaseBlobStorageRepository(ICloudClientWrapper cloudClientWrapper)
        {
            this.cloudClientWrapper = cloudClientWrapper;
        }


        public CloudBlockBlob Get(string fileName, string containerName)
        {
            var container = GetContainer(containerName);
            return container.GetBlockBlobReference(fileName);
        }

        public IEnumerable<IListBlobItem> FindAll(CloudBlobContainer container)
        {
            return container.ListBlobs(null, true);
        }

        public CloudBlobContainer GetContainer(string containerName)
        {
            var container = cloudClientWrapper.BlobClient.GetContainerReference(containerName.ToLower());
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            return container;
        }

    }
}
