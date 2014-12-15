using System.Collections.Generic;
using Microsoft.IT.Geo.Legacy.GeoLocationApi.Common;
using Microsoft.IT.Geo.UpdateRuntime;
using Microsoft.IT.Geo.ObjectModel;

namespace Microsoft.IT.Geo
{
    public interface IGeoLocationService
    {
        GeoLocationLookupResult LookupGeoLocationByIPAddress(string ipAddress, string language);

        GeoLocationLookupResult LookupGeoLocationByIPAddress(string ipAddress, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl);

        GeoLocationLookupResult LookupGeoLocationByMapPointId(int mapPointId, string language, GeoPostalCodeRangeMode postalCodeMode,  string appNameOrRequestedUrl);

        GeoLocationLookupResult LookupGeoLocationFromPostalCode(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl);

        List<GeoEntityInfo> LookupChildListFromId(int id, int languageId);

        GeoLocationDataVersion RetrieveCurrentVersion();

        string RetrieveCacheDataFile();

        GeoLocationRefreshInfo RetrieveDataRefreshDetail();
    }
}
