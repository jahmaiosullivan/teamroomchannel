using Microsoft.IT.Geo.Legacy.GeoCommon;
using Microsoft.IT.Geo.ObjectModel;

namespace Microsoft.IT.Geo.Runtime
{
    public static class GeoLocationInfoFactory
    {
        //=====================================================================
        //  Method: GeoLocationInfoFactory
        //

        //=====================================================================
        //  Method: Create
        //
        /// <summary>
        /// Create a GeoLocationInfo instance from a GeoInfoCommon instance
        /// </summary>  
        //=====================================================================

        internal static GeoLocationInfo Create(
            GeoInfoCommon geoInfoCommon
            )
        {
            GeoLocationInfo geoLocationInfo = null;

            if (geoInfoCommon != null)
            {
                // move this in a factory class or method

                geoLocationInfo = new GeoLocationInfo();

                geoLocationInfo.CityName = geoInfoCommon.CityDisplayName;
                geoLocationInfo.CountryRegionCode = geoInfoCommon.CountryRegionCode;
                geoLocationInfo.CountryRegionName = geoInfoCommon.CountryRegionDisplayName;
                geoLocationInfo.DstEndTimeUniversal = geoInfoCommon.DstEndTimeUniversal;
                geoLocationInfo.DstStartTimeUniversal = geoInfoCommon.DstStartTimeUniversal;
                geoLocationInfo.DstTimeZoneName = geoInfoCommon.DstTimeZoneDisplayName;
                geoLocationInfo.DstTimeZoneOffsetDouble = geoInfoCommon.DstTimeZoneOffset;
                geoLocationInfo.IsEuropeanUnion = geoInfoCommon.IsEuropeanUnion;
                geoLocationInfo.Latitude = geoInfoCommon.Latitude;
                geoLocationInfo.Longitude = geoInfoCommon.Longitude;
                geoLocationInfo.MapPointId = geoInfoCommon.MapPointId;
                geoLocationInfo.MapPointIdParent = geoInfoCommon.MapPointIdParent;
                geoLocationInfo.PostalCodeInfo = geoInfoCommon.PostalCodeInfo;
                // geoLocationInfo.PostalCodes = geoInfoCommon.PostalCodes;
                geoLocationInfo.StandardTimeZoneOffsetDouble = geoInfoCommon.StandardTimeZoneOffset;
                geoLocationInfo.StateProvinceCode = geoInfoCommon.StateProvinceCode;
                geoLocationInfo.StateProvinceName = geoInfoCommon.StateProvinceDisplayName;
                geoLocationInfo.TimeZoneName = geoInfoCommon.TimeZoneDisplayName;
            }

            return geoLocationInfo;
        }
    }
}
