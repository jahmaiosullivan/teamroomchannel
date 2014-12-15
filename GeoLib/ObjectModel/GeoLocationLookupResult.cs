using System.Runtime.Serialization;

namespace Microsoft.IT.Geo.ObjectModel
{
    //=====================================================================
    //  Class: GeoLocationLookupResult
    //
    /// <summary>
    /// The result of a GeoLocationInfo lookup will be returned using this
    /// structure
    /// </summary>
    //=====================================================================

    [DataContract]
    [KnownType(typeof(GeoLocationInfo))]
    public sealed class GeoLocationLookupResult : IExtensibleDataObject
    {
        //=====================================================================
        //  Method: GeoLocationLookupResult
        //
        /// <summary>
        /// Default constructor
        /// </summary>
        //====================================================================

        internal GeoLocationLookupResult(
            GeoLocationInfo geoLocationInfo
            ) : this(geoLocationInfo, false)
        {
        }

        //=====================================================================
        //  Method: GeoLocationLookupResult
        //
        /// <summary>
        /// Default constructor
        /// </summary>
        //====================================================================

        internal GeoLocationLookupResult(
            GeoLocationInfo geoLocationInfo,
            bool isReserved
            )
        {
            this.GeoLocationInfo = geoLocationInfo;
            this.IsReserved = isReserved;
        }


        //=====================================================================
        //  Method: GeoLocationInfo
        //
        /// <summary>
        /// The GeoLocationInfo instance returned by a lookup operation
        /// </summary>
        //====================================================================

        [DataMember(EmitDefaultValue = false)]
        public GeoLocationInfo GeoLocationInfo { get; private set; }

        //=====================================================================
        //  Method: IsReserved
        //
        /// <summary>
        /// Returns information on wheather the IP address used for the lookup
        /// operation represents a reserved address or not;
        /// </summary>
        //====================================================================

        [DataMember(EmitDefaultValue = false)]
        public bool IsReserved { get; private set; }

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
