using GeoLib.Models;
using Microsoft.IT.Geo.Client;

namespace GeoLib
{
    public class GeoService : IGeoService
    {
        private readonly GeoLocationClient client;
        
        public GeoService(string geosegFilePath)
        {
            var configuration = GeoConfiguration.CreateConfiguration();
            configuration.ReadServiceCacheFile = true;

            client = GeoLocationClient.Connect(geosegFilePath, configuration);
        }

        public GeoPlace Get(string ipaddress)
        {
            var geoinfo = Client.LookupGeoLocationByIPAddress(ipaddress, "en-US");
            var place = new GeoPlace();
            if (geoinfo == null || geoinfo.GeoLocationInfo == null) return place;

            place.City =geoinfo.GeoLocationInfo.CityName;
            place.State =geoinfo.GeoLocationInfo.StateProvinceName;
            place.Country =geoinfo.GeoLocationInfo.CountryRegionName;
            return place;
        }

        GeoLocationClient Client { get { return client; } }
    }
}
