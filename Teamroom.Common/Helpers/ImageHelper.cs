using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HobbyClue.Common.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public Image FromFile(string filename)
        {
            return Image.FromFile(filename);
        }

        public Image GetThumbnailImage(Image image, int thumbWidth, int thumbHeight, Image.GetThumbnailImageAbort callback,
            IntPtr callbackData)
        {
            return image.GetThumbnailImage(thumbWidth, thumbHeight, callback, callbackData);
        }

        public void Save(Image image, Stream stream, ImageFormat format)
        {
            image.Save(stream,format);
        }
    }

    public interface IImageHelper
    {
        Image FromFile(string filename);

        Image GetThumbnailImage(Image image, int thumbWidth, int thumbHeight, Image.GetThumbnailImageAbort callback,
            IntPtr callbackData);

        void Save(Image image, Stream stream, ImageFormat format);
    }
}
