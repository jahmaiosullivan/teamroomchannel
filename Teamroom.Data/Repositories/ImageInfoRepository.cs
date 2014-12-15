using HobbyClue.Data.Azure;
using HobbyClue.Data.Azure.Base;

namespace HobbyClue.Data.Repositories
{
    public class ImageInfoRepository : TableStorageRepository<ImageInfo>
    {
        public ImageInfoRepository(ICloudClientWrapper cloudClientWrapper) : base(cloudClientWrapper)
        {
        }
        
        public override string TableName
        {
            get { return "ImageInfo"; }
        }
    }
}
