using System;
using System.Drawing;
using System.IO;
using HobbyClue.Data.Azure;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HobbyClue.Tests.Stubs
{
    public class StubImageRepository : IImageRepository
    {
        public StubImageRepository()
        {
            SavedImageUrl = string.Empty;
            UploadImageStreamResults = string.Empty;
            FileSize = 1;
        }
        public string SavedImageUrl { get; set; }
       public long FileSize { get; set; }

        public string UploadImage(string filePath, out long fileSize, string container = "Images")
        {
            fileSize = FileSize;
            return SavedImageUrl;
        }

        public CloudBlockBlob GetImageBlob(string fileName, string containerName = "Images")
        {
            throw new NotImplementedException();
        }

        public void DownloadImage(string fileName, string outputFilePath)
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(string fileName, string container)
        {
           DeletedImageResult = "Deleted";
        }

        public string DeletedImageResult { get; set; }

        public Image GetImage(string fileName)
        {
            throw new NotImplementedException();
        }

        public string UploadImageStream(MemoryStream fileStream, string filename, string container = "Images")
        {
            return UploadImageStreamResults + filename + container;
        }

        public string UploadImageStreamResults { get; set; }
    }
}
