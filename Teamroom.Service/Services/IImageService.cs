using System.Collections.Generic;
using System.Drawing;
using System.IO;
using HobbyClue.Data.Azure;

namespace HobbyClue.Business.Services
{
    public interface IImageService
    {
        string UploadImage(string filePath, string imageContainer, out string thumbnailUrl);
        Image CreateThumbnail(string fileName, int width, int height);

        MemoryStream CreateThumbnailStream(string fileName, int width, int height);
        Image GetImage(string fileName);

        ImageInfo Get(string fileName);
        
        bool Delete(ImageInfo info);

        string UploadImageThumbNail(string filePath, string imageContainer);

        IEnumerable<ImageInfo> GetListOfAllImages(bool showInactive = false);
    }
}