using System.Drawing;
using System.IO;
using HobbyClue.Data.Azure.Base;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HobbyClue.Data.Azure
{
    public class ImageBlobRepository : BaseBlobStorageRepository, IImageRepository
    {
        protected const string DefaultImageContainer = "Images";
        
        public ImageBlobRepository(ICloudClientWrapper cloudClientWrapper) : base(cloudClientWrapper)
        {
        }

        public string UploadImage(string filePath, out long fileSize, string containerName = null)
        {
            if (string.IsNullOrEmpty(containerName)) containerName = DefaultImageContainer;
            var filename = Path.GetFileName(filePath);
            var blockBlob = GetImageBlob(filename, containerName);
            blockBlob.Properties.ContentType = "image\\jpeg";

            using (var fileStream = File.OpenRead(filePath))
            {
                fileSize = fileStream.Length;
                blockBlob.UploadFromStream(fileStream);
                return blockBlob.SnapshotQualifiedUri.AbsoluteUri;
            }
        }

        public string UploadImageStream(MemoryStream fileStream, string filename, string containerName = null)
        {
            if (string.IsNullOrEmpty(containerName)) containerName = DefaultImageContainer;
            var imageBytes = fileStream.GetBuffer();
            
            using (var imageStream = new MemoryStream(imageBytes))
            {
                var blockBlob = GetImageBlob(filename, containerName);
                blockBlob.Properties.ContentType = "image\\jpeg";
                blockBlob.UploadFromStream(imageStream);
                return blockBlob.SnapshotQualifiedUri.AbsoluteUri;
            }
        }


        public CloudBlockBlob GetImageBlob(string fileName, string containerName = null)
        {
            if (string.IsNullOrEmpty(containerName)) containerName = DefaultImageContainer;
            var container = GetContainer(containerName);
            return container.GetBlockBlobReference(fileName);
        }

        public void DownloadImage(string fileName, string outputFilePath)
        {
            var blockBlob = GetImageBlob(fileName);
            blockBlob.Properties.ContentType = "image\\jpeg";

            // Save blob contents to a file.
            using (var fileStream = File.OpenWrite(outputFilePath))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        public void DeleteImage(string fileName, string container)
        {
            try
            {
                if (string.IsNullOrEmpty(container)) container = DefaultImageContainer;
                var image = GetImageBlob(fileName, container);
                // Delete the blob.
                image.Delete();
            }
            catch (StorageException ex)
            {
                if (ex.Message != "The remote server returned an error: (404) Not Found.")
                    throw;
            }
        }

        public Image GetImage(string fileName)
        {
            var blockBlob = GetImageBlob(fileName);
            blockBlob.Properties.ContentType = "image\\jpeg";

            Image image;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                image = Image.FromStream(memoryStream);
            }

            return image;
        }
    }
}
