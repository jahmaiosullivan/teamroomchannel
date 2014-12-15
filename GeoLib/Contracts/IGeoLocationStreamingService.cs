using System.IO;

namespace Microsoft.IT.Geo.Contracts
{
    public interface IGeoLocationStreamingService
    {
        Stream DownloadGeoLocation();
    }
}
