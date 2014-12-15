using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Microsoft.IT.Geo.Legacy.GeoLocationApi.Common;
using Microsoft.IT.Geo.ObjectModel;
using Microsoft.IT.Geo.UpdateRuntime;

namespace Microsoft.IT.Geo.Client
{

    public class GeoLocationProxy : ClientBase<IGeoLocationService>, IGeoLocationService
    {
        //=====================================================================
        //  Method: GeoProxy
        //
        /// <summary>
        /// This method is the default constructor
        /// </summary>  
        //=====================================================================

        public GeoLocationProxy(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        //=====================================================================
        //  Method: LookupGeoLocationByIPAddress
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified IP address
        /// </summary>  
        //=====================================================================

        public GeoLocationLookupResult LookupGeoLocationByIPAddress(
            string ipAddress,
            string language
            )
        {
            return base.Channel.LookupGeoLocationByIPAddress(
                ipAddress,
                language
                );
        }

        //=====================================================================
        //  Method: LookupGeoLocationByIPAddress with Postal code Mode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified IP address
        /// </summary>  
        //=====================================================================

        public GeoLocationLookupResult LookupGeoLocationByIPAddress(
            string ipAddress,
            string language,
            GeoPostalCodeRangeMode postalCodeMode,
            string appNameOrRequestedUrl)
        {
            return base.Channel.LookupGeoLocationByIPAddress(
                ipAddress,
                language,
                postalCodeMode,
                appNameOrRequestedUrl
                );
        }

        //=====================================================================
        //  Method: LookupGeoLocationByMapPointId with Postal code mode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified MapPoint Id
        /// </summary>  
        //=====================================================================

        public GeoLocationLookupResult LookupGeoLocationByMapPointId(
            int mapPointId,
            string language,
            GeoPostalCodeRangeMode postalCodeMode,
            string appNameOrRequestedUrl)
        {
            return base.Channel.LookupGeoLocationByMapPointId(
                mapPointId,
                language,
                postalCodeMode,
                appNameOrRequestedUrl
                );
        }


        //=====================================================================
        //  Method: LookupGeoLocationFromPostalCode with Postal code mode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified postal code
        /// and country
        /// </summary>  
        //=====================================================================

        public GeoLocationLookupResult LookupGeoLocationFromPostalCode(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl)
        {
            return base.Channel.LookupGeoLocationFromPostalCode(
                        postalCode,
                        countryCode,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
        }


        //=====================================================================
        //  Method: LookupChildListFromId
        //
        /// <summary>
        /// This method returns child list from id 
        /// </summary>  
        //=====================================================================

        public List<GeoEntityInfo> LookupChildListFromId(int id, int languageId)
        {
            return base.Channel.LookupChildListFromId(id, languageId);
        }

        //=====================================================================
        //  Method: RetrieveCurrentVersion
        //
        /// <summary>
        /// This method returns version information for the current Geo data
        /// served by the service
        /// </summary>  
        //=====================================================================

        public GeoLocationDataVersion RetrieveCurrentVersion()
        {
            return base.Channel.RetrieveCurrentVersion();
        }

        //=====================================================================
        //  Method: RetrieveCacheDataFile
        //
        /// <summary>
        /// This method returns cach geo data data file to use
        /// </summary>  
        //=====================================================================

        public string RetrieveCacheDataFile()
        {
            return base.Channel.RetrieveCacheDataFile();
        }

        //=====================================================================
        //  Method: RetrieveDataRefreshDetail
        //
        /// <summary>
        /// This method returns information about the data refresh
        /// process currently in progress (if any in progress)
        /// </summary>  
        //=====================================================================

        public GeoLocationRefreshInfo RetrieveDataRefreshDetail()
        {
            return base.Channel.RetrieveDataRefreshDetail();
        }
    }
}
