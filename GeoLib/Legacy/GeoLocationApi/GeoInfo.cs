using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Microsoft.IT.Geo.Legacy.GeoCommon;
using Microsoft.IT.Geo.Legacy.GeoLocationApi;
using MSCOMGeoSystem.Common;
using System.Globalization;

namespace Microsoft.MSCOM.Geo.GeoLocationAPI
{
	/// <summary>
	/// This class represents the geographic information for a specific CountryRegion, StateProvince, or City.
    /// GeoInfo objects are used to return information from Reverse-IP, MapPointID, andn PostalCode lookups.
    /// They supply information about the location, including its localized name, time zone, latitude, longitude, etc.
	/// </summary>
    public class GeoInfo
    {
		// This is a container class which contains a private geoInfoCommon member from GeoCommon DLL.
		private GeoInfoCommon geoInfoInCommon;

		/// <summary>
		/// Initializes a new instance of the <see cref="GeoInfo"/> class.
		/// </summary>
		public GeoInfo()
		{
			geoInfoInCommon = new GeoInfoCommon();
		}

		internal GeoInfo(GeoInfoCommon geoInfoInCommon)
		{
			if (geoInfoInCommon == null)
			{
				throw new ApplicationException("GeoInfoCommon object cannot be null to create a GeoInfo object.");
			}
			this.geoInfoInCommon = geoInfoInCommon;
		}

		/// <summary>
		/// Gets the internal GeoInfoCommon object.
		/// </summary>
		/// <value>The GeoInfoCommon object within this container class.</value>
		internal GeoInfoCommon GeoInfoCommon
		{
			get
			{
				return this.geoInfoInCommon;
			}
		}

		/// <summary>
		/// Gets the MapPointId of the location, defined by the Microsoft MapPoint taxonomy available at http://alexprobe.
		/// </summary>
        /// <remarks>
        /// The GeoSegmentation system only uses ~13K of the 2.1M MapPointIDs, representing the most popular Cities worldwide.
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public int MapPointId
		{
			get { return geoInfoInCommon.MapPointId; }
			protected internal set { geoInfoInCommon.MapPointId = value; }
		}

		/// <summary>
		/// Gets the MapPointID of the location's parent StateProvince or CountryRegion.
		/// </summary>
        /// <remarks>
        /// If the current MapPointId represents a City, then the ParentID would represent a CountryRegion or StateProvince.
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public int MapPointIdParent
		{
			get { return geoInfoInCommon.MapPointIdParent; }
			protected internal set { geoInfoInCommon.MapPointIdParent = value; }
		}

		/// <summary>
		/// Gets the localized display name of the location's CountryRegion, i.e. "United States" or "Etats Unis".
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public string CountryRegionDisplayName
		{
			get { return geoInfoInCommon.CountryRegionDisplayName; }
			protected internal set { geoInfoInCommon.CountryRegionDisplayName = value; }
		}

		/// <summary>
        /// Gets the two-character ISO 3166-1 alpha-2 code for the location's CountryRegion, i.e. "US".
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public string CountryRegionCode
		{
			get { return geoInfoInCommon.CountryRegionCode; }
			protected internal set { geoInfoInCommon.CountryRegionCode = value; }
		}

		/// <summary>
        /// Gets the localized display name of the location's StateProvince, i.e. "Washington"
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public string StateProvinceDisplayName
		{
			get { return geoInfoInCommon.StateProvinceDisplayName; }
			protected internal set { geoInfoInCommon.StateProvinceDisplayName = value; }
		}

		/// <summary>
        /// Gets two-character code for the location's CountryRegion, i.e. "WA".
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public string StateProvinceCode
        {
			get { return geoInfoInCommon.StateProvinceCode; }
			protected internal set { geoInfoInCommon.StateProvinceCode = value; }
        }

		/// <summary>
        /// Gets the localized display name of the location's City, i.e. "Redmond".
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public string CityDisplayName
		{
			get { return geoInfoInCommon.CityDisplayName; }
			protected internal set { geoInfoInCommon.CityDisplayName = value; }
		}

		/// <summary>
		/// Gets the longitude of the location, supplied by the Microsoft MapPoint team
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public double? Longitude
		{
			get { return geoInfoInCommon.Longitude; }
			protected internal set { geoInfoInCommon.Longitude = value; }
		}

		/// <summary>
        /// Gets the latitude of the location, supplied by the Microsoft MapPoint team.
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public double? Latitude
		{
			get { return geoInfoInCommon.Latitude; }
			protected internal set { geoInfoInCommon.Latitude = value; }
		}

		/// <summary>
		/// Gets the current location's membership in the European Union.
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
        public bool IsEuropeanUnion
        {
			get { return geoInfoInCommon.IsEuropeanUnion; }
			protected internal set { geoInfoInCommon.IsEuropeanUnion = value; }
        }

		/// <summary>
		/// Gets the timezone display name during Standard Time, i.e. "PST".
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public string TimeZoneDisplayName
		{
			get { return geoInfoInCommon.TimeZoneDisplayName; }
			protected internal set { geoInfoInCommon.TimeZoneDisplayName = value; }
		}

		/// <summary>
        /// Gets the timezone display name during Daylight Savings Time, i.e. "PDT".
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public string DstTimeZoneDisplayName
		{
			get { return geoInfoInCommon.DstTimeZoneDisplayName; }
			protected internal set { geoInfoInCommon.DstTimeZoneDisplayName = value; }
		}

		/// <summary>
        /// [Deprecated, use StandardTimeZoneOffsetDouble] Gets the timezone offset from GMT during Standard Time, i.e. -8 for Seattle, WA.
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public int? StandardTimeZoneOffset
		{
			get { return (int?)StandardTimeZoneOffsetDouble; }
			protected internal set { StandardTimeZoneOffsetDouble = value; }
		}

		/// <summary>
        /// [Deprecated, use DstTimeZoneOffsetDouble] Gets the timezone offset from GMT during Daylight Savings Time, i.e. -9 for Seattle, WA.
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public int? DstTimeZoneOffset
		{
            get { return (int?)DstTimeZoneOffsetDouble; }
            protected internal set { DstTimeZoneOffsetDouble = value; }
		}

        /// <summary>
        /// Gets the timezone offset from GMT during Standard Time, i.e. -8 for Seattle, WA.
        /// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
        public double? StandardTimeZoneOffsetDouble
        {
            get { return geoInfoInCommon.StandardTimeZoneOffset; }
            protected internal set { geoInfoInCommon.StandardTimeZoneOffset = value; }
        }

        /// <summary>
        /// Gets the timezone offset from GMT during Daylight Savings Time, i.e. -9 for Seattle, WA.
        /// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
        public double? DstTimeZoneOffsetDouble
        {
            get { return geoInfoInCommon.DstTimeZoneOffset; }
            protected internal set { geoInfoInCommon.DstTimeZoneOffset = value; }
        }

		/// <summary>
		/// Gets the UST DateTime when Daylight Savings Time starts for this location, null if Dst is not used at this location.
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public DateTime? DstStartTimeUniversal
		{
			get { return geoInfoInCommon.DstStartTimeUniversal; }
			protected internal set { geoInfoInCommon.DstStartTimeUniversal = value; }
		}

		/// <summary>
        /// Gets the UST DateTime when Daylight Savings Time ends for this location, null if Dst is not used at this location.
		/// </summary>
        /// <remarks>
        /// The "Set" action is protected and only accessible when overriding this property.
        /// </remarks>
		public DateTime? DstEndTimeUniversal
		{
			get { return geoInfoInCommon.DstEndTimeUniversal; }
			protected internal set { geoInfoInCommon.DstEndTimeUniversal = value; }
		}

		/// <summary>
        /// Gets a string containing a list of the ZipCodes near this location, i.e. "98052+98053-98078", depending on the override used.
		/// </summary>
        /// <remarks>
        /// This value is presented in a compressed format such as "98052-98057" to represent continguous ranges, and "98052+98111" to represent discontiguous ranges.
        /// The "+" and "-" characters used to delimit the ranges are configurable.
        /// [Note] This property is only populated for United States locations when requested.
        /// </remarks>
		public string PostalCodeInfo
		{
			get { return geoInfoInCommon.PostalCodeInfo; }
			protected internal set { geoInfoInCommon.PostalCodeInfo = value; }
		}

		/// <summary>
        /// Gets a collection of strings, one for each entry in PostalCodeInfo.
        /// </summary>
        /// <remarks>
		/// If PostalcodeInfo contains "98052+98063-98065", this method returns a string collection with 98052, 98063, 98064, 98065. 
		/// This property might have negative performance impact if PostalCodeInfo has a long list of codes. So please only use it as necessary.
		/// Note: 
		/// 1. '+' is the main seperator. The whole string is split by + into parts.
		/// 2. For each part, if it does not have exactly one '-', or it has other non-numeric characters, just returns it as-is.
		/// 3. For a part with exactly one '-' and all numerical characters, the method returns the whole range. But if the range start value > end value, it returns it as-is (it usually won't happen)
		/// [Note] '+' is configured as postalCodeSeperatorCharacter, '-' is configured as postalCodeRangeSeperatorCharacter
        /// [Note] PostalCodeInfo is only populated for United States locations.
		/// </remarks>
		public ReadOnlyCollection<string> PostalCodes
		{
			get
			{
				return geoInfoInCommon.PostalCodes;
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		/// <remarks>
		/// Returns all the properties of the GeoInfo object in one string. Primarily used for Unit Tests and won't be used in applications.
		/// </remarks>
		public override string ToString()
		{
			return geoInfoInCommon.ToString();
		}

        /// <summary>
        /// [New in V2] Gets core (i.e. MapPointID, CountryRegionCode, etc.) and user-defined properties of the GeoInfo object.
        /// </summary>
        /// <remarks>
        /// For example: myGeoInfo["MapPointId"], myGeoInfo["MapPointIDParent"], etc.
        /// Please refer to the SDK for how to configure Extended Properties.
        /// </remarks>
        /// <param name="name">Non-case sensitive name of the core or extended/configurable property, i.e. "MapPointId"</param>
        /// <returns>String value for the requested property, or null if property name is not found</returns>
        public string this[string name]
        {
            get
            {
                if ( string.IsNullOrEmpty(name))
                    return null;
                else 
                    return GetPropertyValue(name);
            }
        }

        /// <summary>
        /// Private helper method to return a string property value for a GeoInfo object
        /// </summary>
        /// <param name="propName">Non-case sensitive name of the core or extended/configurable property, i.e. "MapPointId"</param>
        /// <returns>string property Value for available properties, returns null if property is not found </returns>
        private string GetPropertyValue(string propName)
        {

            try
            {

                if (string.IsNullOrEmpty(propName))
                    return null;

                string name = propName.ToLower(CultureInfo.InvariantCulture);
                //Check if Extended Property 
                ExtProperty extProp = ExtendedProperties.Properties[name] as ExtProperty;

                if (extProp != null)
                {
                    
                    // is extended property, check if this geoInfo object has been configured
                    if (extProp.Regions.Count < 1 || extProp.Values.Count < 1)
                    {
                        return extProp.DefaultValue;
                    }

                    string key = null;
                    if (extProp.Regions.TryGetValue(this.MapPointId.ToString(CultureInfo.InvariantCulture), out key))
                    {

                        return extProp.Values[key] != null ? extProp.Values[key].ToString() : extProp.DefaultValue;

                    }
                    else if (extProp.Regions.TryGetValue(this.CountryRegionCode + "\\" + this.StateProvinceCode, out key))
                    {

                        return extProp.Values[key] != null ? extProp.Values[key].ToString() : extProp.DefaultValue;

                    }
                    else if (extProp.Regions.TryGetValue(this.CountryRegionCode, out key))
                    {

                        return extProp.Values[key] != null ? extProp.Values[key].ToString() : extProp.DefaultValue;

                    }
                    else
                    {
                        return extProp.DefaultValue;
                    }
                }//extended property.

                //property is one of the core fields, get it from geoInfoCommon
                switch (name)
                {
                    case "citydisplayname":
                        return this.CityDisplayName;

                    case "countryregioncode":
                        return this.CountryRegionCode;

                    case "countryregiondisplayname":
                        return this.CountryRegionDisplayName;

                    case "dstendtimeuniversal":
                        return (this.DstEndTimeUniversal != null ? this.DstEndTimeUniversal.ToString() : null);

                    case "dststarttimeuniversal":
                        return (this.DstStartTimeUniversal != null ? this.DstStartTimeUniversal.ToString() : null);

                    case "dsttimezonedisplayname":
                        return this.DstTimeZoneDisplayName;

                    case "dsttimezoneoffset":
                        return (this.DstTimeZoneOffset != null ) ? this.DstTimeZoneOffset.ToString() : null;

                    case "dsttimezoneoffsetdouble":
                        return ( this.DstTimeZoneOffsetDouble != null) ? this.DstTimeZoneOffsetDouble.ToString() : null;

                    case "iseuropeanunion":
                        return this.IsEuropeanUnion.ToString();

                    case "latitude":
                        return (this.Latitude != null) ? this.Latitude.ToString() : null;

                    case "longitude":
                        return (this.Longitude != null) ? this.Longitude.ToString() : null;

                    case "mappointid":
                        return this.MapPointId.ToString(CultureInfo.InvariantCulture);

                    case "mappointidparent":
                        return this.MapPointIdParent.ToString(CultureInfo.InvariantCulture);

                    //case "postalcodeinfo":
                    //  return this.PostalCodeInfo;

                    //case "postalcodes":
                    //  return this.PostalCodes.ToString();

                    case "standardtimezoneoffset":
                        return (this.StandardTimeZoneOffset != null) ? this.StandardTimeZoneOffset.ToString() : null;

                    case "standardtimezoneoffsetdouble":
                        return (this.StandardTimeZoneOffsetDouble != null) ? this.StandardTimeZoneOffsetDouble.ToString() : null;

                    case "stateprovincecode":
                        return this.StateProvinceCode;

                    case "stateprovincedisplayname":
                        return this.StateProvinceDisplayName;

                    case "timezonedisplayname":
                        return this.TimeZoneDisplayName;

                    default:
                        return null;

                }

            }
            catch
            {
                return null;
            }
        } 
    }
}
