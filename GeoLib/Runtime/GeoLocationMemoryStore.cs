using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Microsoft.IT.Geo.Legacy.GeoCommon;
using Microsoft.IT.Geo.Legacy.GeoLocationApi.Common;
using Microsoft.IT.Geo.ObjectModel;
using Microsoft.IT.Geo.UpdateRuntime;

namespace Microsoft.IT.Geo.Runtime
{
    //=========================================================================
    //  Class: GeoLocationMemoryStore
    //
    /// <summary>
    /// In-Memory store for GeoLocation data
    /// </summary>
    //=========================================================================

    internal sealed class GeoLocationMemoryStore
    {
        private GeoInfoLookup geoInfoLookup;

        //=====================================================================
        //  Method:GeoLocationMemoryStore
        //
        /// <summary>
        /// Default constructor.
        /// </summary>  
        //=====================================================================

        private GeoLocationMemoryStore()
        {
        }

        //=====================================================================
        //  Method:DataVersion
        //
        /// <summary>
        /// The version information for the Geo Location data stored
        /// in this instance
        /// </summary>  
        //=====================================================================

        public GeoLocationDataVersion DataVersion { get; private set; }

        //=====================================================================
        //  Method:SourceFilePath
        //
        /// <summary>
        /// The file path where this geo location data was loaded from;
        /// if this in-memory data, was loaded not from a file, this will
        /// return null
        /// </summary>  
        //=====================================================================

        public string SourceFilePath { get; private set; }

        //=====================================================================
        //  Method:Load
        //
        /// <summary>
        /// Loads the geo data from the input stream.
        /// </summary>  
        //=====================================================================

        public static GeoLocationMemoryStore Load(
            Stream dataStream
            )
        {
            GeoLocationMemoryStore locationStore = null;

            if (dataStream != null)
            {
                locationStore = new GeoLocationMemoryStore();

                using (dataStream)
                {
                    locationStore.geoInfoLookup = new GeoInfoLookup(dataStream);
                }
            }
            else
            { 
                throw new ArgumentException(
                    "Cannot load Geo Location data from a null stream.",
                    "dataStream"
                    );
            }
            return locationStore;
        }

        //=====================================================================
        //  Method:Load
        //
        /// <summary>
        /// Loads the geo data from the input stream.
        /// </summary>  
        //=====================================================================

        public static GeoLocationMemoryStore Load(
            string cacheFilePath
            )
        {
            GeoLocationMemoryStore locationStore;

            if (string.IsNullOrEmpty(cacheFilePath) == false)
            {
                locationStore = new GeoLocationMemoryStore();

                var aRequest = (HttpWebRequest)WebRequest.Create(cacheFilePath);
                var aResponse = (HttpWebResponse)aRequest.GetResponse();
                locationStore.geoInfoLookup = new GeoInfoLookup(aResponse.GetResponseStream());
                    
                locationStore.SourceFilePath = cacheFilePath;
            }
            else
            {
                throw new ArgumentException(
                    "Cannot load Geo Location data from a null or empty file path.",
                    "cacheFilePath"
                    );
            }
            return locationStore;
        }

        //=====================================================================
        //  Method:Load
        //
        /// <summary>
        /// Loads the geo data from the input stream.
        /// </summary>  
        //=====================================================================

        public static GeoLocationMemoryStore LoadFromDatabase()
        {
            GeoLocationMemoryStore locationStore = null;

            
            locationStore = new GeoLocationMemoryStore
            {
                geoInfoLookup = new GeoInfoLookup(null)
            };

            locationStore.DataVersion = ComputeVersion(locationStore.geoInfoLookup);

            return locationStore;
        }

        //=====================================================================
        //  Method:Write
        //
        /// <summary>
        /// Writes the in-memory data to the specified file
        /// </summary>  
        //=====================================================================

        public void Write(
            string outputFilePath
            )
        {
            Debug.Assert(this.geoInfoLookup != null);

            if (string.IsNullOrEmpty(outputFilePath) == false)
            {

                FileStream outputFileStream = null;
                BinaryWriter dataWriter = null;

                try
                {
                    outputFileStream = File.Open(
                        outputFilePath,
                        FileMode.OpenOrCreate
                        );

                    using (dataWriter = new BinaryWriter(outputFileStream))
                    {
                        this.geoInfoLookup.Write(
                            dataWriter
                            );
                    }
                }
                finally
                {
                    if (outputFileStream != null)
                    {
                        outputFileStream.Close();
                    }
                }
            }
            else
            {
                throw new ArgumentException(
                    "Cannot write Geo Location to a null/empty file path.",
                    "outputFilePath"
                    );
            }
        }

        //=====================================================================
        //  Method:Write
        //
        /// <summary>
        /// Writes the in-memory data to the specified file
        /// </summary>  
        //=====================================================================

        public void Write(Stream geoStream)
        {
            if (geoStream != null)
            {
                BinaryWriter geoInfoWriter = new BinaryWriter(
                    geoStream
                    );

                this.geoInfoLookup.Write(
                    geoInfoWriter
                    );

                //
                //  move the pointer to the begining of the stream
                //  as the caller will fail to read the stream, and also
                //  the caller cannot Seek
                //

                geoStream.Seek(0, SeekOrigin.Begin);
            }
            else
            {
                throw new ArgumentNullException(
                    "geoStream",
                    "Cannot write Geo Location to a null stream."
                    );
            }
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
            GeoLocationLookupResult lookupResult = null;
            GeoLocationInfo geoLocationInfo = null;

            //
            //  First verify if this a reserved address
            //  case in which we do not do any other lookup
            //

            bool isReserved = this.geoInfoLookup.IsReserved(
                ipAddress
                );

            if (isReserved == false)
            {
                GeoInfoCommon geoInfoCommon = this.geoInfoLookup.GetGeoInfoByIP(
                            ipAddress,
                            language,
                            (int)GeoPostalCodeRangeMode.None
                            );

                geoLocationInfo = GeoLocationInfoFactory.Create(
                    geoInfoCommon
                    );

                lookupResult = new GeoLocationLookupResult(
                    geoLocationInfo
                    );
            }
            else
            {
                lookupResult = new GeoLocationLookupResult(
                    null,
                    isReserved
                    );
            }

            return lookupResult;
        }

        #region Geo 2.0 Methods support

        #region LookupGeoLocationByIPAddress

        /// <summary>
        /// Returns GeoInfo objects by passing in a string with an IPAddress, i.e. "192.1.2.3".
        /// </summary>
        /// <param name="ipAddress">IP address for lookup in the format "192.1.2.3"</param>
        /// <param name="language">The language to use in the format "en-us"</param>
        /// <param name="appNameOrRequestedUrl">The name of the application or page URL to pass to back-end health monitoring, normally set to context.Request.Path</param>
        /// <returns>
        /// A GeoInfo object for the user's location, or null if no location can be determined
        /// </returns>
        /// <remarks>
        /// Makes an off-box call in the out-of-process configuration when the Windows Service is down, and the SQL database is configured.
        /// </remarks>

        //with Postal code 
        public GeoLocationLookupResult LookupGeoLocationByIPAddress(
          string ipAddress,
          string language,
          GeoPostalCodeRangeMode postalCodeMode,
          string appNameOrRequestedUrl)
        {
            GeoLocationLookupResult lookupResult = null;
            GeoLocationInfo geoLocationInfo = null;

            //
            //  First verify if this a reserved address
            //  case in which we do not do any other lookup
            //

            bool isReserved = this.geoInfoLookup.IsReserved(
                ipAddress
                );

            if (isReserved == false)
            {
                GeoInfoCommon geoInfoCommon = this.geoInfoLookup.GetGeoInfoByIP(
                            ipAddress,
                            language,
                            (int)postalCodeMode
                            );
                geoLocationInfo = GeoLocationInfoFactory.Create(
                    geoInfoCommon
                    );

                lookupResult = new GeoLocationLookupResult(
                    geoLocationInfo
                    );
            }
            else
            {
                lookupResult = new GeoLocationLookupResult(
                    null,
                    isReserved
                    );
            }

            return lookupResult;
        }


        #endregion

        #region LookupGeoLocationByMapPointId

        /// <summary>
        /// Returns GeoInfo objects by passing in an integer with a MapPointID, i.e. 244 is the MapPointID for the United States.
        /// </summary>
        /// <param name="mapPointId">MappointID for the lookup, see http://alexprobe for the complete list of MapPointIDs, however GeoSegmentation only supports the most-popular 13K cities worldwide</param>
        /// <param name="language">The language to use in the format "en-us"</param>
        /// <param name="appNameOrRequestedUrl">The name of the application or page URL to pass to back-end health monitoring, normally set to context.Request.Path</param>
        /// <returns>
        /// A GeoInfo object for the user's location, or null if no location can be determined.
        /// </returns>
        /// <remarks>
        /// Makes an off-box call in the out-of-process configuration when the Windows Service is down, and the SQL database is configured.
        /// </remarks>
        //with Postal code 
        public GeoLocationLookupResult LookupGeoLocationByMapPointId(int mapPointId, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl)
        {
            GeoLocationLookupResult lookupResult = null;
            GeoLocationInfo geoLocationInfo = null;

            GeoInfoCommon geoInfoCommon = this.geoInfoLookup.GetGeoInfoByMappointId(
                            mapPointId,
                            language,
                            (int)postalCodeMode
                            );
            geoLocationInfo = GeoLocationInfoFactory.Create(
                     geoInfoCommon
                     );

            lookupResult = new GeoLocationLookupResult(
                geoLocationInfo
                );

            return lookupResult;
        }

        #endregion


        #region LookupGeoLocationFromPostalCode

        /// <summary>
        /// [Updated in V2] Returns GeoInfo objects given a CountryCode ("US") and a PostalCode ("98052").
        /// </summary>
        /// <param name="postalCode">PostalCode to lookup in the format "98052"</param>
        /// <param name="countryCode">CountryCode of the PostalCode in the format "US"</param>
        /// <param name="language">The language to use in the format "en-us"</param>
        /// <param name="postalCodeMode">Controls whether the GeoInfo should populate the Postalcode string. For fastest performance, use GeoPostalCodeRange.None.</param>
        /// <param name="appNameOrRequestedUrl">The name of the application or page URL to pass to back-end health monitoring, normally set to context.Request.Path</param>
        /// <returns>
        /// A GeoInfo object for the user's location, or null if no location can be determined
        /// </returns>
        /// <remarks>
        /// Makes an off-box call in the out-of-process configuration when the Windows Service is down, and the SQL database is configured.
        /// [Note] In V1, this method always went off-box to the SQL Server to fetch this data.
        /// </remarks>
        public GeoLocationLookupResult LookupGeoLocationFromPostalCode(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl)
        {
            GeoLocationLookupResult lookupResult = null;
            GeoLocationInfo geoLocationInfo = null;

            GeoInfoCommon geoInfoCommon = this.geoInfoLookup.GetGeoInfoByPostalCode(
                        postalCode,
                        countryCode,
                        language,
                       (int)postalCodeMode
                        );
            geoLocationInfo = GeoLocationInfoFactory.Create(
                     geoInfoCommon
                     );

            lookupResult = new GeoLocationLookupResult(
                geoLocationInfo
                );

            return lookupResult;
        }

        #endregion





        #region LookupChildListFromId


        public List<GeoEntityInfo> LookupChildListFromId(int id, int languageId)
        {
            List<GeoEntityInfo> lookupResult = null;

            var geoEntity = this.geoInfoLookup.GetChildListFromId(id, languageId);

            lookupResult = GeoEntityInfoFactory.Create(geoEntity);
            return lookupResult;
        }

        #endregion

        #endregion

        //=====================================================================
        //  Method: Clear
        //
        /// <summary>
        /// Cleans up teh data in this cache
        /// </summary>  
        //=====================================================================

        public void Clear()
        {
            if (this.geoInfoLookup != null)
            {
                this.geoInfoLookup.Clear();
            }
        }

        private static GeoLocationDataVersion ComputeVersion(
            GeoInfoLookup geoData
            )
        {
            GeoLocationDataVersion dataVersion = null;

            if (geoData != null)
            {

                MemoryStream dataStream = null;

                try
                {
                    dataStream = new MemoryStream();

                    using (BinaryWriter dataWriter = new BinaryWriter(dataStream))
                    {

                        geoData.Write(dataWriter);

                        dataWriter.Seek(0, SeekOrigin.Begin);

                        dataVersion = GeoLocationDataVersion.ComputeVersion(
                            dataStream
                            );
                    }
                }
                finally
                {
                    dataStream.Close();
                }
            }
            else
            {
                throw new ArgumentNullException(
                    "geoData",
                    "Cannot compute version of a null instance of geo data."
                    );
            }

            return dataVersion;
        }
    }
}
