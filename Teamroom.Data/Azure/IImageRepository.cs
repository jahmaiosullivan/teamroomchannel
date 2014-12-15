using System.Drawing;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HobbyClue.Data.Azure
{
    public interface IImageRepository
    {
        string UploadImage(string filePath, out long fileSize, string container = "Images");
        CloudBlockBlob GetImageBlob(string fileName, string containerName = "Images");
        void DownloadImage(string fileName, string outputFilePath);
        void DeleteImage(string fileName, string container);
        Image GetImage(string fileName);

        string UploadImageStream(MemoryStream fileStream, string filename, string container = "Images");
    }
}