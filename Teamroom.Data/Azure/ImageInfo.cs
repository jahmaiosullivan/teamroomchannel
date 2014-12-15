using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace HobbyClue.Data.Azure
{
    public class ImageInfo : TableEntity
    {
        public ImageInfo()
        {
            PartitionKey = "ImageInfo";
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                RowKey = value + "_" + Guid.NewGuid();
                name = value;
            }
        }

        public string ThumbnailUrl { get; set; }

        public string Url { get; set; }

        public string Error { get; set; }

        public string Size { get; set; }
     
        public bool IsActive { get; set; }

        public bool IsPrivate { get; set; }

        public string Container { get; set; }
    }
}