using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.IT.Geo.Client;
using Microsoft.IT.Geo.Legacy.GeoLocationApi.Common;
using Microsoft.IT.Geo.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using Microsoft.IT.Geo.Runtime;
using Microsoft.IT.Geo.UpdateRuntime;

namespace Microsoft.IT.Geo
{
    //=====================================================================
    //  Class: FromServiceGeoRefreshProcessor
    //
    /// <summary>
    /// Class responsible for refreshing Geo data by verifying if a new
    /// version is available from the geo local service
    /// </summary>
    //=====================================================================

    public sealed class FromServiceGeoRefreshProcessor
    {
        private static object internalLock = new object();

        private static int initialized = 0;
        
        //
        //  This is the synchronization point that prevents
        //  the Refresh method from running concurrently
        //

        private static int syncPoint = 0;

        private GeoLocationDataVersion currentVersion;

        private String currentCacheFilePath;

        private GeoRefreshScheduler refreshScheduler;

        private static GeoLocationMemoryStore locationMemoryCache;

        private static GeoConfiguration config;

        //=====================================================================
        //  Method: FromServiceGeoRefreshProcessor
        //
        /// <summary>
        /// Default constructor
        /// </summary>  
        //=====================================================================

        private FromServiceGeoRefreshProcessor()
        {
        }

        //=====================================================================
        //  Method: FromServiceGeoRefreshProcessor
        //
        /// <summary>
        /// Parameterized constructor
        /// </summary>  
        //=====================================================================

        private FromServiceGeoRefreshProcessor(
            GeoLocationDataVersion currentGeoLocationVersion,
            GeoRefreshConfiguration refreshConfiguration,
            GeoConfiguration serviceConfiguration
            )
        {
            Debug.Assert(serviceConfiguration != null);

            this.CurrentVersion = currentGeoLocationVersion;

            if (refreshConfiguration != null)
            {
                this.refreshScheduler = GeoRefreshScheduler.Create(
                    this.RefreshGeoData,
                    refreshConfiguration
                    );
            }

            FromServiceGeoRefreshProcessor.config = serviceConfiguration;
        }

        //=====================================================================
        //  Method: CurrentVersion
        //
        /// <summary>
        /// This method returns the version information for the current
        /// Geo Data instance
        /// </summary>  
        //=====================================================================

        public GeoLocationDataVersion CurrentVersion
        {
            get
            {
                lock (FromServiceGeoRefreshProcessor.internalLock)
                {
                    return this.currentVersion;
                }
            }
            set
            {
                lock (FromServiceGeoRefreshProcessor.internalLock)
                {
                    this.currentVersion = value;
                }
            }
        }

        //=====================================================================
        //  Method: CurrentCacheFilePath
        //
        /// <summary>
        /// This method returns the cahe file path for the current
        /// Geo Data instance
        /// </summary>  
        //=====================================================================

        public string CurrentCacheFilePath
        {
            get
            {
                lock (FromServiceGeoRefreshProcessor.internalLock)
                {
                    return this.currentCacheFilePath;
                }
            }
            set
            {
                lock (FromServiceGeoRefreshProcessor.internalLock)
                {
                    this.currentCacheFilePath = value;
                }
            }
        }

        //=====================================================================
        //  Method: Create
        //
        /// <summary>
        /// This method creates an instance of the GeoFileDownloadProcessor
        /// after it validates the input parameters
        /// </summary>  
        //=====================================================================

        public static FromServiceGeoRefreshProcessor Create(
            GeoLocationDataVersion currentGeoLocationVersion,
            GeoRefreshConfiguration refreshConfiguration,
            GeoConfiguration serviceConfiguration
            )
        {
            FromServiceGeoRefreshProcessor processorInstance = null;

            if (serviceConfiguration != null)
            {
                //
                //  We update the service connection configuration
                //  to ensure that we do not use some client options
                //  for caching
                //

                serviceConfiguration = serviceConfiguration.Clone();

                serviceConfiguration.UseServiceCacheFile = false;
                serviceConfiguration.ClientAutoRefresh = false;

                processorInstance = new FromServiceGeoRefreshProcessor(
                    currentGeoLocationVersion,
                    refreshConfiguration,
                    serviceConfiguration
                    );
            }
            else
            {
                throw new ArgumentNullException(
                    "serviceConfiguration",
                    "Cannot initialize a refresh processor for the Geo data without info for connecting to the Geo services."
                    );
            }

            return processorInstance;
        }

        //=====================================================================
        //  Method: Start
        //
        /// <summary>
        /// This method starts the processor
        /// </summary>
        //=====================================================================

        public void Start()
        {
            //
            //  Make sure this processor is initialized
            //  before we start it
            //

            this.Initialize();

            //
            //  Refresh scheduler can be null, if 
            //  no auto refresh function is needed from this processor
            //

            if (this.refreshScheduler != null)
            {

                this.refreshScheduler.Start();
            }
        }

        //=====================================================================
        //  Method: Stop
        //
        /// <summary>
        /// This method stops the processor
        /// </summary>
        //=====================================================================

        public void Stop()
        {
            //
            //  Refresh scheduler can be null, if 
            //  no auto refresh function is needed from this processor
            //

            if (this.refreshScheduler != null)
            {

                this.refreshScheduler.Stop();
            }
        }

        //=====================================================================
        //  Method: RefreshDownload
        //
        /// <summary>
        /// This method is the one called on a scheduled basis for performing
        /// the Geo Location data file refresh, by using the new data file
        /// if present
        /// </summary>
        //=====================================================================

        public void RefreshGeoData(object value)
        {
            //
            //  Ensure that we do not execute this
            //  multiple times concurrently
            //

            int sync = Interlocked.CompareExchange(
                ref FromServiceGeoRefreshProcessor.syncPoint,
                1,
                0
                );

            if (sync == 0)
            {
                //
                //  Performan the Download new file phase of 
                //  the general Two Phase Refresh
                //

                try
                {
                    this.ProcessNewData();
                }
                finally
                {
                    //
                    //  Release control of syncPoint to let 
                    //  the next refreshes perform the work
                    //

                    syncPoint = 0;

                    Interlocked.CompareExchange(ref syncPoint,0,1);
                }
            }
        } // RefreshDownload method

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
            GeoLocationLookupResult lookupResult = null;

            if (FromServiceGeoRefreshProcessor.locationMemoryCache != null)
            {
                lookupResult = FromServiceGeoRefreshProcessor.locationMemoryCache.LookupGeoLocationByIPAddress(
                    ipAddress,
                    language
                    );
            }
            else
            {
                lookupResult = new GeoLocationLookupResult(
                    null
                    );
            }

            return lookupResult;
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
            string language,
            GeoPostalCodeRangeMode postalCodeMode,
            string appNameOrRequestedUrl
            )
        {
            GeoLocationLookupResult lookupResult = null;

            if (FromServiceGeoRefreshProcessor.locationMemoryCache != null)
            {
                lookupResult = FromServiceGeoRefreshProcessor.locationMemoryCache.LookupGeoLocationByIPAddress(
                    ipAddress,
                    language,
                    postalCodeMode,
                    appNameOrRequestedUrl
                    );
            }
            else
            {
                lookupResult = new GeoLocationLookupResult(
                    null
                    );
            }

            return lookupResult;
        }

        //=====================================================================
        //  Method: LookupGeoLocationByMapPointId with Postal code 
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified MapPoint Id
        /// </summary>  
        //=====================================================================
        public GeoLocationLookupResult LookupGeoLocationByMapPointId(
          int mapPointId,
          string language,
          GeoPostalCodeRangeMode postalCodeMode,
          string appNameOrRequestedUrl
          )
        {
            GeoLocationLookupResult lookupResult = null;

            if (FromServiceGeoRefreshProcessor.locationMemoryCache != null)
            {
                lookupResult = FromServiceGeoRefreshProcessor.locationMemoryCache.LookupGeoLocationByMapPointId(
                    mapPointId,
                    language,
                    postalCodeMode,
                    appNameOrRequestedUrl
                    );
            }
            else
            {
                lookupResult = new GeoLocationLookupResult(
                    null
                    );
            }

            return lookupResult;
        }

        //=====================================================================
        //  Method: LookupGeoLocationFromPostalCode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified 
        /// postal code and country
        /// </summary>  
        //=====================================================================

        public GeoLocationLookupResult LookupGeoLocationFromPostalCode(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl)
        {
            GeoLocationLookupResult lookupResult = null;

            if (FromServiceGeoRefreshProcessor.locationMemoryCache != null)
            {
                lookupResult = FromServiceGeoRefreshProcessor.locationMemoryCache.LookupGeoLocationFromPostalCode(
                        postalCode,
                        countryCode,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
            }
            else
            {
                lookupResult = new GeoLocationLookupResult(
                    null
                    );
            }

            return lookupResult;
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
            List<GeoEntityInfo> lookupResult = null;

            if (FromServiceGeoRefreshProcessor.locationMemoryCache != null)
            {
                lookupResult = FromServiceGeoRefreshProcessor.locationMemoryCache.LookupChildListFromId(id, languageId);
            }
            else
            {
                lookupResult = new List<GeoEntityInfo>();
            }

            return lookupResult;
        }

        //=====================================================================
        //  Method: DownloadGeoLocation
        //
        /// <summary>
        /// This method returns geo location data as a stream, containing
        /// all the data in one blob
        /// </summary>  
        //=====================================================================

        public Stream DownloadGeoLocation()
        {
            Debug.Assert(
                FromServiceGeoRefreshProcessor.locationMemoryCache != null
                );

            MemoryStream geoInfoStream = null;
            
            geoInfoStream = new MemoryStream();

            FromServiceGeoRefreshProcessor.locationMemoryCache.Write(
                geoInfoStream
                );
            
            return geoInfoStream;
        }

        //=====================================================================
        //  Method: Initialize
        //
        /// <summary>
        /// This method performs the initialization for the processor, which
        /// includes: getting the first version and loading it in memory,
        /// </summary>
        //=====================================================================

        private void Initialize()
        {
            if (!config.ReadServiceCacheFile) return;
            var latestGeoCache = GeoLocationMemoryStore.Load(config.ServiceCacheFilePath);
            locationMemoryCache = latestGeoCache;
        }

        

        //=====================================================================
        //  Method: ProcessNewData
        //
        /// <summary>
        /// This method read the geo data from teh database, and, if different
        /// than the current one it will make it the current one
        /// </summary>
        //=====================================================================

        public GeoLocationDataVersion ProcessNewData()
        {
            
            if (FromServiceGeoRefreshProcessor.config.ReadServiceCacheFile)
            {
                try
                {
                    FileInfo fileInfo = new DirectoryInfo(FromServiceGeoRefreshProcessor.config.ServiceCacheFolderPath).GetFiles("Geo*.dat").OrderByDescending(d => d.LastWriteTimeUtc).First();
                    if (fileInfo != null)
                    {
                        if (FromServiceGeoRefreshProcessor.config.ServiceCacheFilePath != fileInfo.FullName)
                        {
                            FromServiceGeoRefreshProcessor.config.UseServiceCacheFile = true;
                            FromServiceGeoRefreshProcessor.config.ServiceCacheFilePath = fileInfo.FullName;
                            GeoLocationMemoryStore latestGeoCache = GeoLocationMemoryStore.Load(
                                      FromServiceGeoRefreshProcessor.config.ServiceCacheFilePath
                                     );

                            //
                            //  We loaded it successful, so we switch
                            //

                            FromServiceGeoRefreshProcessor.locationMemoryCache = latestGeoCache;
                            string message = string.Format(CultureInfo.InvariantCulture, "Geo Data latest version {0} refreshed successfully",
                                FromServiceGeoRefreshProcessor.config.ServiceCacheFilePath);

                            //EventLogging.WriteMessage(
                            //    message,
                            //    EventLogEntryType.Information,
                            //    GeoDiagnosticsMessageId.GeoLocationClientCacheRefreshSuccess
                            //    );


                        }
                        return null;
                    }
                    else
                    {
                        //EventLogging.WriteEntry(new LogEntry("Invalid datafile or file doesn't exist"));
                        return null;
                    }
                }
                catch
                {
                    //EventLogging.WriteEntry(new LogEntry("Invalid datafolder path or invalid datafile or file doesn't exist"));
                    return null;
                }
            }
           

            //
            //  Get the version of the latest data
            //

            GeoLocationDataVersion latestVersion = FromServiceGeoRefreshProcessor.GetCurrentVersion();

            //
            //  We switch to this new geo data if the version
            //  is different than the current one
            //

            if (latestVersion != this.CurrentVersion)
            {
                //
                //  There is a different data version
                //  so we switch to that one
                //

                string cacheFilePath = FromServiceGeoRefreshProcessor.GetCurrentCacheDataFile();
                
                if (string.IsNullOrEmpty(cacheFilePath) == false)
                {
                    //
                    //  Verify if the file exists
                    //

                    if (File.Exists(cacheFilePath) == true)
                    {
                       
                            GeoLocationMemoryStore latestGeoCache = GeoLocationMemoryStore.Load(
                                cacheFilePath
                                );

                            //
                            //  We loaded it successful, so we switch
                            //

                            locationMemoryCache = latestGeoCache;

                            CurrentVersion = latestVersion;
                            CurrentCacheFilePath = cacheFilePath;

                    }
                    else
                    {
                        // write an error in event log
                        string errorText = "Cannot find Geo Data cache file: " + cacheFilePath;

                        //EventLogging.WriteError(
                        //    errorText,
                        //    GeoDiagnosticsMessageId.GeoLocationClientCacheRefreshError
                        //    );

                        //TraceLogging.WriteText(
                        //    errorText,
                        //    TraceLevel.Error
                        //    );
                    }
                }

                FromServiceGeoRefreshProcessor.LogRefreshSuccessMessage(
                    this.CurrentVersion
                    );
            }
            else
            {
                latestVersion = null;
            }

            return latestVersion;
        } // RetrieveNewData

        internal void Uninitialize()
        {
            //
            //  Re-set to uninitialize; we should not do this here
            //  as the initialized field should be instance based
            //  as currently a Stop on one instance, will stop
            //  and make all instances not startable again
            //  Also, there is instance level data, CurrentVersion,
            //  which is only initialized based on the static "initialized" field
            //  so there is no way to re-initialize it again, once created a new instance
            //  Not a problem in prod, as only one instance of this will be created
            //  but still something to clean up
            //

            Interlocked.CompareExchange(
                ref initialized,
                0,
                1
                );
        }

        private static GeoLocationDataVersion GetCurrentVersion()
        {
            GeoLocationDataVersion dataVersion = null;

            GeoLocationClient locationClient = null;
            try
            {
                locationClient = FromServiceGeoRefreshProcessor.ConstructGeoLocationClient();

                dataVersion = locationClient.RetrieveCurrentVersion();
            }
            finally
            {
                if (locationClient != null)
                {
                    locationClient.Close();
                    locationClient = null;
                }
            }            

            return dataVersion;
        }

        private static string GetCurrentCacheDataFile()
        {
            string cacheFilePath = null;

            GeoLocationClient locationClient = null;
            try
            {
                locationClient = FromServiceGeoRefreshProcessor.ConstructGeoLocationClient();

                cacheFilePath = locationClient.RetrieveCacheDataFile();
            }
            finally
            {
                if (locationClient != null)
                {
                    locationClient.Close();
                    locationClient = null;
                }
            }

            return cacheFilePath;
        }

        private static GeoLocationClient ConstructGeoLocationClient()
        {
            GeoLocationClient locationClient = null;

            if (FromServiceGeoRefreshProcessor.config.ReadServiceCacheFile)
            {
                locationClient = GeoLocationClient.Connect(FromServiceGeoRefreshProcessor.config.ServiceCacheFolderPath);
            }
            else
            {
                locationClient = GeoLocationClient.Connect(
                    FromServiceGeoRefreshProcessor.config
                    );
            }

            return locationClient;
        }

        private static void LogRefreshSuccessMessage(
            GeoLocationDataVersion latestVersion
            )
        {
            Debug.Assert(latestVersion != null);
            
            string latestVersionValue = string.Empty;

            if (latestVersion != null)
            {
                latestVersionValue = latestVersion.ToString();
            }

            string message = string.Format(
                CultureInfo.InvariantCulture,
                "Geo Data latest version {0} refreshed successfully",
                latestVersionValue
                );

            //TraceLogging.WriteText(
            //    message,
            //    global::System.Diagnostics.TraceLevel.Info
            //    );

            //EventLogging.WriteMessage(
            //    message,
            //    EventLogEntryType.Information,
            //    GeoDiagnosticsMessageId.GeoLocationClientCacheRefreshSuccess
            //    );
        }

        private static void LogRefreshFailedMessage(
            GeoLocationDataVersion latestVersion,
            Exception ex
            )
        {
            string latestVersionValue = string.Empty;

            if (latestVersion != null)
            {
                latestVersionValue = latestVersion.ToString();
            }

            string message = string.Format(
                CultureInfo.InvariantCulture,
                "Geo Data latest version {0} refresh failed.",
                latestVersionValue
                );

            //TraceLogging.WriteText(
            //    message,
            //    global::System.Diagnostics.TraceLevel.Error
            //    );

            //EventLogging.WriteMessage(
            //    message,
            //    EventLogEntryType.Error,
            //    GeoDiagnosticsMessageId.GeoLocationClientCacheRefreshError
            //    );

            if (ex != null)
            {
                //EventLogging.WriteException(
                //    ex,
                //    GeoDiagnosticsMessageId.GeoLocationClientCacheRefreshError
                //    );
            }
        }
    }
}
