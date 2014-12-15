using System;
using System.Runtime.Serialization;

namespace Microsoft.IT.Geo.UpdateRuntime
{
    ///=========================================================================
    ///  Class: GeoLocationRefreshState
    /// 
    /// <summary>
    /// This class contains the state information for a geo location data
    /// refresh process
    /// </summary>
    ///=========================================================================

    [DataContract]
    public enum GeoLocationRefreshState
    {
        /// <summary>
        /// This is the default value, for a
        /// refresh process that is not started
        /// </summary>
        
        [EnumMember]
        None,

        /// <summary>
        /// the data refresh process is started
        /// </summary>

        [EnumMember]
        Started,

        [EnumMember]
        Successful,

        /// <summary>
        /// the data refresh is being downloaded
        /// </summary>

        [EnumMember]
        Failed
    }

    //=========================================================================
    //  Class: GeoLocationRefreshInfo
    //
    /// <summary>
    /// This class exposes information about the geo location data refresh
    /// state
    /// </summary>
    //=========================================================================

    [DataContract]
    [Serializable]
    [KnownType(typeof(GeoLocationDataVersion))]
    [KnownType(typeof(GeoLocationRefreshState))]
    public sealed class GeoLocationRefreshInfo
    {
        [DataMember(EmitDefaultValue=false)]
        public GeoLocationDataVersion CurrentVersion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool IsRefreshPending { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public GeoLocationDataVersion PendingRefreshVersion { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public GeoLocationRefreshState RefreshProcessState { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? ProposedSwitchTime { get; set; }
    }
}
