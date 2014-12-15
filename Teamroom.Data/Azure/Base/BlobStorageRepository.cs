namespace HobbyClue.Data.Azure.Base
{
    public class BlobStorageRepository : BaseBlobStorageRepository, IBlobStorageRepository
    {
        public BlobStorageRepository(ICloudClientWrapper cloudClientWrapper) : base(cloudClientWrapper)
        {
        }
    }
}