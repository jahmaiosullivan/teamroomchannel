using System;
using System.Configuration;

namespace Microsoft.IT.Geo.UpdateRuntime
{
    //=========================================================================
    //  Class: GeoRefreshConfiguration
    //
    /// <summary>
    /// This class contains app.config settings for Geo data Update logic
    /// </summary>
    //=========================================================================

    public sealed class GeoRefreshConfiguration
    {
        private const string GeoDataRefreshUtcTimeKey = "GeoDataRefreshUtcTime";

        private const string GeoDataRefreshIntervalKey = "GeoDataRefreshInterval";

        private static readonly TimeSpan DefaultGeoDataRefreshInterval = new TimeSpan(24, 0, 0);

        private static DateTime DefaultGeoDataRefreshUtcTime
        {
            get
            {
                return new DateTime(
                    DateTime.UtcNow.Year,
                    DateTime.UtcNow.Month,
                    DateTime.UtcNow.Day,
                    0,
                    0,
                    0,
                    DateTimeKind.Utc
                    );
            }
        }

        //=====================================================================
        //  Method: GeoRefreshConfiguration
        //
        /// <summary>
        /// This is the class default constructor.
        /// </summary>  
        //=====================================================================

        private GeoRefreshConfiguration()
            : this(
                GeoRefreshConfiguration.GetGeoDataRefreshUtcTime(),
                GeoRefreshConfiguration.GetGeoDataRefreshInterval()
                )
        {
        }

        //=====================================================================
        //  Method: GeoRefreshConfiguration
        //
        /// <summary>
        /// This is the class parameterized constructor.
        /// </summary>  
        //=====================================================================

        private GeoRefreshConfiguration(
            DateTime refreshTime,
            TimeSpan refreshInterval
            )
        {
            if (refreshTime.Kind == DateTimeKind.Utc)
            {
                if (refreshInterval != null)
                {
                    this.GeoDataRefreshUtcTime = refreshTime;
                    this.GeoDataRefreshInterval = refreshInterval;
                }
                else
                {
                    throw new ArgumentNullException(
                        "refreshInterval",
                        "Required configuration for refresh interval is missing."
                        );
                }
            }
            else
            {
                throw new ArgumentException(
                    "Refresh time needs to be a UTC time.",
                    "refreshTime"
                    );
            }
        }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This method returns the default configuration
        /// </summary>  
        //=====================================================================

        public static GeoRefreshConfiguration CreateConfiguration()
        {
            GeoRefreshConfiguration configuration = new GeoRefreshConfiguration();

            return configuration;
        }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This method returns the default configuration
        /// </summary>  
        //=====================================================================

        public static GeoRefreshConfiguration CreateConfiguration(
            DateTime refreshTime,
            TimeSpan refreshInterval
            )
        {
            GeoRefreshConfiguration configuration = new GeoRefreshConfiguration(
                refreshTime,
                refreshInterval
                );

            return configuration;
        }

        //=====================================================================
        //  Method: GeoDataRefreshUtcTime
        //
        /// <summary>
        /// This method returns the Universal time information when the service
        /// will attempt a Geo data refresh
        /// </summary>  
        //=====================================================================

        public DateTime GeoDataRefreshUtcTime { get; private set; }

        //=====================================================================
        //  Method: GeoDataRefreshInterval
        //
        /// <summary>
        /// This method returns the time interval between the geo data
        /// refresh processes
        /// </summary>  
        //=====================================================================

        public TimeSpan GeoDataRefreshInterval { get; private set; }

        //=====================================================================
        //  Method: GetGeoDataRefreshUtcTime
        //
        /// <summary>
        /// This method returns the GetGeoDataRefreshUtcTime value from configuration.
        /// </summary>  
        //=====================================================================

        private static DateTime GetGeoDataRefreshUtcTime(
            )
        {
            DateTime refreshTime = GeoRefreshConfiguration.DefaultGeoDataRefreshUtcTime;

            string configEntry = ConfigurationManager.AppSettings[GeoRefreshConfiguration.GeoDataRefreshUtcTimeKey];

            if (string.IsNullOrEmpty(configEntry) != false)
            {
                bool isValidTime = DateTime.TryParse(
                    configEntry,
                    out refreshTime
                    );

                if (isValidTime == false)
                {
                    refreshTime = GeoRefreshConfiguration.DefaultGeoDataRefreshUtcTime;
                }
                else
                {
                    refreshTime = new DateTime(
                        DateTime.UtcNow.Year,
                        DateTime.UtcNow.Month,
                        DateTime.UtcNow.Day,
                        refreshTime.Hour,
                        refreshTime.Minute,
                        refreshTime.Second,
                        refreshTime.Millisecond,
                        DateTimeKind.Utc
                        );
                }
            }

            return refreshTime;
        }

        //=====================================================================
        //  Method: GetGeoDataRefreshInterval
        //
        /// <summary>
        /// This method returns the GetGeoDataRefreshInterval value from configuration.
        /// </summary>  
        //=====================================================================

        private static TimeSpan GetGeoDataRefreshInterval(
            )
        {
            TimeSpan refreshInterval = GeoRefreshConfiguration.DefaultGeoDataRefreshInterval;

            string configEntry = ConfigurationManager.AppSettings[GeoRefreshConfiguration.GeoDataRefreshIntervalKey];

            if (string.IsNullOrEmpty(configEntry) != false)
            {
                bool isValidTime = TimeSpan.TryParse(
                    configEntry,
                    out refreshInterval
                    );

                if (isValidTime == false)
                {
                    refreshInterval = GeoRefreshConfiguration.DefaultGeoDataRefreshInterval;
                }
               
            }

            return refreshInterval;
        }
    }
}
