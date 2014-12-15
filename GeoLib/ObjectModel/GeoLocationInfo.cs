using System;
using System.Runtime.Serialization;

namespace Microsoft.IT.Geo.ObjectModel
{
    //=====================================================================
    //  Class: GeoLocationInfo
    //
    /// <summary>
    /// Geographic information for a specific CountryRegion, StateProvince,
    /// or City.
    /// GeoLocationInfo objects are used to return information from 
    /// Reverse-IP, MapPointID, andn PostalCode lookups.
    /// They supply information about the location, including its localized
    /// name, time zone, latitude, longitude, etc.
    /// </summary>
    //=====================================================================

    [DataContract]
    public sealed class GeoLocationInfo : IExtensibleDataObject
    {
        //=====================================================================
        //  Method: MapPointId
        //
        /// <summary>
		/// Gets the MapPointId of the location, defined by the Microsoft
        /// MapPoint taxonomy
		/// </summary>
        //====================================================================

        [DataMember(EmitDefaultValue=false)]
        public int MapPointId { get; internal set; }

        //=====================================================================
        //  Method: MapPointIdParent
        //
        /// <summary>
		/// Gets the MapPointID of the location's parent StateProvince or
        /// CountryRegion.
		/// </summary>
        /// <remarks>
        /// If the current MapPointId represents a City, then the ParentID
        /// would represent a CountryRegion or StateProvince.
        /// </remarks>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public int MapPointIdParent { get; internal set; }

        //=====================================================================
        //  Method: CountryRegionName
        //
        /// <summary>
		/// Gets the localized display name of the location's CountryRegion,
        /// i.e. "United States" or "Etats Unis".
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string CountryRegionName { get; internal set; }

        //=====================================================================
        //  Method: CountryRegionCode
        //
        /// <summary>
        /// Gets the two-character ISO 3166-1 alpha-2 code for the location's
        /// CountryRegion, i.e. "US".
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string CountryRegionCode { get; internal set; }

        //=====================================================================
        //  Method: StateProvinceName
        //
        /// <summary>
        /// Gets the localized display name of the location's StateProvince,
        /// i.e. "Washington"
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string StateProvinceName { get; internal set; }

        //=====================================================================
        //  Method: StateProvinceCode
        //
        /// <summary>
        /// Gets two-character code for the location's CountryRegion, i.e. "WA".
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string StateProvinceCode { get; internal set; }

        //=====================================================================
        //  Method: CityName
        //
        /// <summary>
        /// Gets the localized display name of the location's City, i.e
        /// . "Redmond".
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string CityName { get; internal set; }
		
		//=====================================================================
        //  Method: Longitude
        //
        /// <summary>
		/// Gets the longitude of the location, supplied by the 
        /// Microsoft MapPoint team
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public double? Longitude { get; internal set; }

        //=====================================================================
        //  Method: Latitude
        //
        /// <summary>
        /// Gets the latitude of the location, supplied by the Microsoft
        /// MapPoint team.
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public double? Latitude { get; internal set; }


        //=====================================================================
        //  Method: IsEuropeanUnion
        //
        /// <summary>
		/// Gets the current location's membership in the European Union.
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public bool IsEuropeanUnion { get; internal set; }

        //=====================================================================
        //  Method: TimeZoneDisplayName
        //
        /// <summary>
		/// Gets the timezone display name during Standard Time, i.e. "PST".
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string TimeZoneName { get; internal set; }


        //=====================================================================
        //  Method: DstTimeZoneDisplayName
        //
        /// <summary>
        /// Gets the timezone display name during Daylight Savings Time, i.e.
        /// "PDT".
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string DstTimeZoneName { get; internal set; }

        //=====================================================================
        //  Method: StandardTimeZoneOffsetDouble
        //
        /// <summary>
        /// Gets the timezone offset from GMT during Standard Time,
        /// i.e. -8 for Seattle, WA.
        /// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public double? StandardTimeZoneOffsetDouble { get; internal set; }

        //=====================================================================
        //  Method: DstTimeZoneOffsetDouble
        //
        /// <summary>
        /// Gets the timezone offset from GMT during Daylight Savings Time,
        /// i.e. -9 for Seattle, WA.
        /// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public double? DstTimeZoneOffsetDouble { get; internal set; }

        //=====================================================================
        //  Method: DstStartTimeUniversal
        //
        /// <summary>
		/// Gets the UST DateTime when Daylight Savings Time starts for this
        /// location, null if Dst is not used at this location.
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public DateTime? DstStartTimeUniversal { get; internal set; }

        //=====================================================================
        //  Method: DstEndTimeUniversal
        //
        /// <summary>
        /// Gets the UST DateTime when Daylight Savings Time ends for this
        /// location, null if Dst is not used at this location.
		/// </summary>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public DateTime? DstEndTimeUniversal { get; internal set; }

        //=====================================================================
        //  Method: PostalCodeInfo
        //
        /// <summary>
        /// Gets a string containing a list of the ZipCodes near this location,
        /// i.e. "98052+98053-98078", depending on the override used.
		/// </summary>
        /// <remarks>
        /// This value is presented in a compressed format such as "98052-98057"
        /// to represent continguous ranges, and "98052+98111" to represent
        /// discontiguous ranges.
        /// The "+" and "-" characters used to delimit the ranges are 
        /// configurable.
        /// [Note] This property is only populated for United States locations
        /// when requested.
        /// </remarks>
        //=====================================================================

        [DataMember(EmitDefaultValue = false)]
        public string PostalCodeInfo { get; internal set; }

        //=====================================================================
        //  Method: PostalCodes
        //
        /// <summary>
        /// Gets a collection of strings, one for each entry in PostalCodeInfo.
        /// </summary>
        /// <remarks>
		/// If PostalcodeInfo contains "98052+98063-98065", this method returns
        /// a string collection with 98052, 98063, 98064, 98065. 
		/// This property might have negative performance impact if
        /// PostalCodeInfo has a long list of codes. So please only use it
        /// as necessary.
		/// Note: 
		/// 1. '+' is the main seperator. The whole string is split by + into
        /// parts.
		/// 2. For each part, if it does not have exactly one '-', or it has
        /// other non-numeric characters, just returns it as-is.
		/// 3. For a part with exactly one '-' and all numerical characters,
        /// the method returns the whole range. 
        /// But if the range start value > end value, it returns it as-is
        /// (it usually won't happen)
		/// [Note] '+' is configured as postalCodeSeperatorCharacter, '-' is
        /// configured as postalCodeRangeSeperatorCharacter
        /// [Note] PostalCodeInfo is only populated for United States locations.
		/// </remarks>
        //=====================================================================

        //[DataMember(EmitDefaultValue = false)]
        //public ReadOnlyCollection<string> PostalCodes { get; internal set; } 

        //=====================================================================
        //  Method: ExtensionData
        ///
        /// <summary>
        /// This method implements IExtensibleDataObject.ExtensionData
        /// </summary>
        //=====================================================================

        public ExtensionDataObject ExtensionData { get; set; }
    }
}
