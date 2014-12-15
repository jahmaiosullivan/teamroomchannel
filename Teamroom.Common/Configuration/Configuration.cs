using System.Configuration;

namespace Teamroom.Common.Configuration
{
    public class Configuration : IConfiguration
    {
        public string StorageConnectionString { get; set; }
        public string FacebookAppId { get { return ConfigurationManager.AppSettings["FbAppId"]; }  }
        public string FacebookAppSecret { get { return ConfigurationManager.AppSettings["FbAppSecret"]; } }
        public string GeoSegFile { get { return ConfigurationManager.AppSettings["GeoSegFile"]; } }
    }
}
