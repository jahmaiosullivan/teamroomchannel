using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using HobbyClue.Common.Helpers;
using HobbyClue.Data.Azure;
using HobbyClue.Data.Azure.Base;

namespace HobbyClue.Business.Services
{
    public class ImageService : IImageService
    {
        protected readonly IImageRepository _imageRepository;
        protected readonly ITableStorageRepository<ImageInfo> _imageInfoRepository;
        protected readonly IImageHelper _imageHelper;
        public const int ThumbnailWidthHeightInPx = 250;

        public ImageService(IImageRepository imageRepository, ITableStorageRepository<ImageInfo> imageInfoRepository, IImageHelper imageHelper)
        {
            _imageRepository = imageRepository;
            _imageInfoRepository = imageInfoRepository;
            _imageHelper = imageHelper;
        }

        public virtual string UploadImage(string filePath, string imageContainer, out string thumbnailUrl)
        {
            long uploadedFileSize;
            thumbnailUrl = UploadImageThumbNail(filePath, imageContainer);
            return _imageRepository.UploadImage(filePath, out uploadedFileSize, imageContainer);
        }


        public ImageInfo Get(string fileName)
        {
            return _imageInfoRepository.Find("Id", fileName);
        }

        public bool Delete(ImageInfo info)
        {
            info.IsActive = false;
            info.IsPrivate = false;

            var isDeleted = _imageInfoRepository.Update(info);
            if (isDeleted)
                _imageRepository.DeleteImage(info.Name, info.Container);

            return isDeleted;
        }

        public virtual string UploadImageThumbNail(string filePath, string imageContainer)
        {
              if (string.IsNullOrEmpty(filePath)) return null;
              var thumbnailStream = CreateThumbnailStream(filePath);
              var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
              var thumbnailFilename = "thumbnail_" + fileNameWithoutExtension + Path.GetExtension(filePath);
              
              var thumbNailUrl = _imageRepository.UploadImageStream(thumbnailStream, thumbnailFilename, imageContainer);
              return thumbNailUrl;
        }

        public virtual Image CreateThumbnail(string fileName, int width = ThumbnailWidthHeightInPx, int height = ThumbnailWidthHeightInPx)
        {
            var imageStream = CreateThumbnailStream(fileName, width, height);
            var thumbnail = ConvertBytesToImage(imageStream.ToArray());
            return thumbnail;
        }


        public virtual MemoryStream CreateThumbnailStream(string fileName, int width = ThumbnailWidthHeightInPx, int height = ThumbnailWidthHeightInPx)
        {
            using (var image = _imageHelper.FromFile(fileName))
            {
                // Create a Thumbnail from image with size 50x40.
                // Change 50 and 40 with whatever size you want
                using (var thumbPhoto = _imageHelper.GetThumbnailImage(image, width, height, null, new IntPtr()))
                {
                    // The below code converts an Image object to a byte array
                    using (var ms = new MemoryStream())
                    {
                        _imageHelper.Save(thumbPhoto, ms, ImageFormat.Jpeg);
                        return ms;
                    }
                }
            }
        }


        private static Image ConvertBytesToImage(byte[] byteArrayIn)
        {
            var ms = new MemoryStream(byteArrayIn);
            var returnImage = Image.FromStream(ms);
            return returnImage;
        }


        public Image GetImage(string fileName)
        {
            var blockBlob = _imageRepository.GetImageBlob(fileName);
            blockBlob.Properties.ContentType = "image\\jpeg";

            Image image;
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);
                image = Image.FromStream(memoryStream);
            }

            return image;
        }

        public IEnumerable<ImageInfo> GetListOfAllImages(bool showInactive = false)
        {
            var imageInfos = _imageInfoRepository.Find();
            if (imageInfos != null && !showInactive)
                imageInfos = imageInfos.Where(x => x.IsActive).AsEnumerable();
            return imageInfos;
        }
    }
}
