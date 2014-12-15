using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Threading;
using System.Web;
using Microsoft.IT.Geo.Hosting;
using Microsoft.IT.Geo.Legacy.GeoLocationApi;
using Microsoft.IT.Geo.Legacy.GeoLocationApi.Common;
using Microsoft.IT.Geo.ObjectModel;
using Microsoft.IT.Geo.UpdateRuntime;
using Microsoft.MSCOM.Geo.GeoLocationAPI;

namespace Microsoft.IT.Geo.Client
{
    //=========================================================================
    //  Class: GeoClient
    //
    /// <summary>
    /// This class is the GeoService service client-side API
    /// </summary>
    //=========================================================================

    public sealed class GeoLocationClient : IGeoLocationService
    {
        private static int initialized;

        private readonly GeoConfiguration config;

        private static FromServiceGeoRefreshProcessor serviceRefreshProcessor;

        //=====================================================================
        //  Method:GeoLocationClient
        //
        /// <summary>
        /// This is the constructor for this class.
        /// </summary>  
        //=====================================================================

        private GeoLocationClient(GeoConfiguration configuration)
        {
            Debug.Assert(configuration != null);

            this.config = configuration;

            this.Initialize();
        }
        
        //=====================================================================
        //  Method: GeoLocationProxy
        //
        /// <summary>
        /// This method sets or sets the geo location proxy
        /// </summary>  
        //=====================================================================

        private GeoLocationProxy GeoLocationProxy { get; set; }


        
        //=====================================================================
        //  Method:Connect
        //
        /// <summary>
        /// This static method creates an instance of GeoClient
        /// using connection/service settings from application configuration 
        /// file.
        /// </summary>  
        //=====================================================================

        public static GeoLocationClient Connect()
        {
            GeoConfiguration configuration = GeoConfiguration.CreateConfiguration();

            return GeoLocationClient.Connect(
                configuration
                );
        }

        //=====================================================================
        //  Method:Connect
        //
        /// <summary>
        /// This static method creates an instance of GeoClient
        /// using data file path provided as param.
        /// </summary>  
        //=====================================================================

        public static GeoLocationClient Connect(string dataFolderPath)
        {
            try
            {
                GeoConfiguration configuration = GeoConfiguration.CreateConfiguration();
                return Connect(dataFolderPath, configuration);
            }
            catch
            {
               // EventLogging.WriteEntry(new LogEntry("Invalid datafolder path or invalid datafile or file doesn't exist"));
                return new GeoLocationClient(GeoConfiguration.CreateConfiguration());
            }
        }

        //=====================================================================
        //  Method:Connect
        //
        /// <summary>
        /// This static method creates an instance of GeoClient
        /// using data file path and Configuration provided as param.
        /// </summary>  
        //=====================================================================

        public static GeoLocationClient Connect(string filePath, GeoConfiguration configuration)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(filePath))
                {
                  
                    configuration.UseServiceCacheFile = true;
                    configuration.ServiceCacheFilePath = filePath;
                }
                return new GeoLocationClient(configuration);


            }
            catch
            {
                //EventLogging.WriteEntry(new LogEntry("Invalid datafolder path or invalid datafile or file doesn't exist or invalid configuration"));
                return new GeoLocationClient(GeoConfiguration.CreateConfiguration());
            }
        }

        //=====================================================================
        //  Method: Connect
        //
        /// <summary>
        /// This method connects to the Geo Service.
        /// </summary>  
        //=====================================================================

        public static GeoLocationClient Connect(GeoConfiguration configuration)
        {
            var client = new GeoLocationClient(configuration);            

            if (client.GeoLocationProxy != null)
            {
                ClientProxy.OpenProxy(client.GeoLocationProxy);
            }
            
            return client;
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

            if (serviceRefreshProcessor != null)
            {
                lookupResult = serviceRefreshProcessor.LookupGeoLocationByIPAddress(ipAddress, language);
            }
            else
            {
                //
                //  We always fall back to calling the service, if there
                //  is something wrong with the cached data
                //

                lookupResult = this.LookupGeoLocationByIPAddressAutonomous(ipAddress, language);
            }

            return lookupResult;
        }

        //=====================================================================
        //  Method: LookupGeoLocationByIPAddressAutonomous
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified IP address
        /// using a new geo proxy, opened and closed, inside this call
        /// </summary>  
        //=====================================================================

        private GeoLocationLookupResult LookupGeoLocationByIPAddressAutonomous(
            string ipAddress,
            string language
            )
        {
            GeoLocationProxy locationProxy = null;

            GeoLocationLookupResult lookupResult = null;

            try
            {
                locationProxy = GeoLocationClient.CreateGeoLocationProxy(
                    this.config
                    );


                if (locationProxy != null)
                {
                    lookupResult = locationProxy.LookupGeoLocationByIPAddress(
                        ipAddress,
                        language
                        );
                }
            }
            finally
            {
                if (locationProxy != null)
                {
                    GeoLocationClient.CloseGeoLocationProxy(
                        locationProxy
                        );
                }
            }

            return lookupResult;
        }

        //=====================================================================
        //  Method: TryLookupGeoLocationByIPAddress
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified IP address
        /// </summary>  
        //=====================================================================

        public bool TryLookupGeoLocationByIPAddress(
            string ipAddress,
            string language,
            out GeoLocationLookupResult lookupResult
            )
        {
            bool operationSuccessful = false;

            lookupResult = null;

            try
            {
                //
                //  We do not make the call if this is not a valid
                //  IP address
                //
                IPAddress parsedAddress = null;

                operationSuccessful = IPAddress.TryParse(ipAddress, out parsedAddress);

                if (operationSuccessful != false)
                {
                    lookupResult = this.LookupGeoLocationByIPAddress(
                        ipAddress,
                        language
                        );

                    operationSuccessful = true;
                }
            }
            catch (Exception)
            {
                operationSuccessful = false;
            }

            return operationSuccessful;
        }


        #region Geo Upgrade section


        #region LookupGeoLocationByIPAddress
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
            string appNameOrRequestedUrl)
        {
            return LookupGeoLocationByIPAddress(
                          ipAddress,
                          language,
                          GeoPostalCodeRangeMode.None,
                          appNameOrRequestedUrl
                          );
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
            string appNameOrRequestedUrl)
        {
            GeoLocationLookupResult lookupResult = null;

            if (this.config.UseServiceCacheFile != false)
            {
                Debug.Assert(this.GeoLocationProxy == null);
                
                if (serviceRefreshProcessor != null){
                    lookupResult = serviceRefreshProcessor.LookupGeoLocationByIPAddress(ipAddress,language,postalCodeMode,appNameOrRequestedUrl);
                }
                else
                {
                    //
                    //  We always fall back to calling the service, if there
                    //  is something wrong with the cached data
                    //

                    lookupResult = this.LookupGeoLocationByIPAddressAutonomous(
                        ipAddress,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
            }
            else
            {
                Debug.Assert(this.GeoLocationProxy != null);
                
                lookupResult = this.GeoLocationProxy.LookupGeoLocationByIPAddress(ipAddress,language,postalCodeMode,appNameOrRequestedUrl);
            }

            return lookupResult;
        }

        //=====================================================================
        //  Method: LookupGeoLocationByIPAddressAutonomous
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified IP address
        /// using a new geo proxy, opened and closed, inside this call
        /// </summary>  
        //=====================================================================

        private GeoLocationLookupResult LookupGeoLocationByIPAddressAutonomous(
            string ipAddress,
            string language,
            GeoPostalCodeRangeMode postalCodeMode,
            string appNameOrRequestedUrl
            )
        {
            GeoLocationProxy locationProxy = null;

            GeoLocationLookupResult lookupResult = null;

            try
            {
                locationProxy = GeoLocationClient.CreateGeoLocationProxy(
                    this.config
                    );


                if (locationProxy != null)
                {
                    lookupResult = locationProxy.LookupGeoLocationByIPAddress(
                        ipAddress,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
            }
            finally
            {
                if (locationProxy != null)
                {
                    GeoLocationClient.CloseGeoLocationProxy(
                        locationProxy
                        );
                }
            }

            return lookupResult;
        }

        #endregion

        #region LookupGeoLocationByMapPointId
        //=====================================================================
        //  Method: LookupGeoLocationByMapPointId
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified MapPoint Id
        /// </summary>  
        //=====================================================================

        public GeoLocationLookupResult LookupGeoLocationByMapPointId(
            int mapPointId,
            string language,
            string appNameOrRequestedUrl)
        {
            return LookupGeoLocationByMapPointId(
                        mapPointId,
                        language,
                        GeoPostalCodeRangeMode.None,
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
            GeoLocationLookupResult lookupResult = null;

            if (this.config.UseServiceCacheFile != false)
            {
                Debug.Assert(this.GeoLocationProxy == null);

                if (GeoLocationClient.serviceRefreshProcessor != null)
                {
                    lookupResult = GeoLocationClient.serviceRefreshProcessor.LookupGeoLocationByMapPointId(
                        mapPointId,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
                else
                {
                    //
                    //  We always fall back to calling the service, if there
                    //  is something wrong with the cached data
                    //

                    lookupResult = this.LookupGeoLocationByMapPointIdAutonomous(
                        mapPointId,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
            }
            else
            {
                Debug.Assert(this.GeoLocationProxy != null);

                lookupResult = this.GeoLocationProxy.LookupGeoLocationByMapPointId(
                    mapPointId,
                    language,
                    postalCodeMode,
                    appNameOrRequestedUrl
                    );
            }

            return lookupResult;
        }


        //=====================================================================
        //  Method: LookupGeoLocationByMapPointIdAutonomous with Postal code mode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified MapPoint Id
        /// using a new geo proxy, opened and closed, inside this call
        /// </summary>  
        //=====================================================================

        private GeoLocationLookupResult LookupGeoLocationByMapPointIdAutonomous(
            int mapPointId,
            string language,
            GeoPostalCodeRangeMode postalCodeMode,
            string appNameOrRequestedUrl
            )
        {
            GeoLocationProxy locationProxy = null;

            GeoLocationLookupResult lookupResult = null;

            try
            {
                locationProxy = GeoLocationClient.CreateGeoLocationProxy(
                    this.config
                    );


                if (locationProxy != null)
                {
                    lookupResult = locationProxy.LookupGeoLocationByMapPointId(
                        mapPointId,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
            }
            finally
            {
                if (locationProxy != null)
                {
                    GeoLocationClient.CloseGeoLocationProxy(
                        locationProxy
                        );
                }
            }

            return lookupResult;
        }

        #endregion

        #region LookupGeoLocationFromPostalCode
        //=====================================================================
        //  Method: LookupGeoLocationFromPostalCode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified Postal Code
        /// Country
        /// </summary>  
        //=====================================================================
        public GeoLocationLookupResult LookupGeoLocationFromPostalCode(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl)
        {
            GeoLocationLookupResult lookupResult = null;

            if (this.config.UseServiceCacheFile != false)
            {
                Debug.Assert(this.GeoLocationProxy == null);

                if (GeoLocationClient.serviceRefreshProcessor != null)
                {
                    lookupResult = GeoLocationClient.serviceRefreshProcessor.LookupGeoLocationFromPostalCode(
                        postalCode,
                        countryCode,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
                else
                {
                    //
                    //  We always fall back to calling the service, if there
                    //  is something wrong with the cached data
                    //

                    lookupResult = this.LookupGeoLocationFromPostalCodeAutonomous(
                        postalCode,
                        countryCode,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
            }
            else
            {
                Debug.Assert(this.GeoLocationProxy != null);

                lookupResult = this.GeoLocationProxy.LookupGeoLocationFromPostalCode(
                        postalCode,
                        countryCode,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
            }

            return lookupResult;
        }


        //=====================================================================
        //  Method: LookupGeoLocationByMapPointIdAutonomous
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified MapPoint Id
        /// using a new geo proxy, opened and closed, inside this call
        /// </summary>  
        //=====================================================================

        private GeoLocationLookupResult LookupGeoLocationFromPostalCodeAutonomous(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalCodeMode, string appNameOrRequestedUrl)
        {
            GeoLocationProxy locationProxy = null;

            GeoLocationLookupResult lookupResult = null;

            try
            {
                locationProxy = GeoLocationClient.CreateGeoLocationProxy(
                    this.config
                    );


                if (locationProxy != null)
                {
                    lookupResult = locationProxy.LookupGeoLocationFromPostalCode(
                        postalCode,
                        countryCode,
                        language,
                        postalCodeMode,
                        appNameOrRequestedUrl
                        );
                }
            }
            finally
            {
                if (locationProxy != null)
                {
                    GeoLocationClient.CloseGeoLocationProxy(
                        locationProxy
                        );
                }
            }

            return lookupResult;
        }


         //=====================================================================
        //  Method: LookupGeoLocationFromPostalCode with out postal code mode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified Postal Code
        /// Country
        /// </summary>  
        //=====================================================================
        public GeoLocationLookupResult LookupGeoLocationFromPostalCode(string postalCode, string countryCode, string language, string appNameOrRequestedUrl)
        {

            return LookupGeoLocationFromPostalCode(
                      postalCode,
                      countryCode,
                      language,
                      GeoPostalCodeRangeMode.None,
                      appNameOrRequestedUrl
                      );
        }

        #endregion

        #region LookupGeoLocationByHttpRequest

        //VIJAY TODo CHECK !!!!

        //   <!-- The name of the parameter in the http request query string for MapPoint ID.-->
        //<setting name ="MapPointIDQueryStringName" value ="MPID" />
        // <setting name="NetscalerIPHeader" value="client-ip"></setting>

        private const string AnonymousCookieName = "A"; // Same as defined in Profile Provider
        private const string GeoCookieName = "GO"; // Same as defined in Profile Provider

        private string mapPointIdQueryStringName = "MPID";
        public string MapPointIdQueryStringName
        {
            get 
            {

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["MapPointIDQueryStringName"]))
                {
                    return ConfigurationManager.AppSettings["MapPointIDQueryStringName"];
                }
                else
                {
                    return mapPointIdQueryStringName;
                }
            }
        }

        private string netscalerIPHeader = "client-ip";
        public string NetscalerIPHeader
        {
            get {

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["NetscalerIPHeader"]))
                {
                    return ConfigurationManager.AppSettings["NetscalerIPHeader"];
                }
                else
                {
                    return netscalerIPHeader;
                } 
            }
        }


        /// <summary>
        /// [New in V2] Returns GeoInfo objects from HTTPRequests.
        /// </summary>
        /// <param name="httpRequest">The HTTPRequest object, normally a reference to context.Request</param>
        /// <returns>
        /// A GeoInfo object for the user's location, or null if no location can be determined
        /// </returns>
        /// <remarks>
        /// This override will use the Accept-Lang from the request as the language.
        /// Lookup order of precendence:
        /// <list type="bullet">
        /// 		<item>MapPointId from QueryString</item>
        /// 		<item>MapPointId from Cookie</item>
        /// 		<item>IP Address from a NetScaler-added header</item>
        /// 		<item>IP Address from HttpRequest.UserHostAddress</item>
        /// 		<item>If IP Address is '127.0.0.1', it will use the first Ethernet card's IP Address</item>
        /// 	</list>
        /// Will first look for a MapPointID in the QueryString, and then in the Cookie.
        /// If no MapPointID is found, then it will use the HTTPRequest's IP Address.
        /// It uses the Accept-Lang from the HTTPRequest as the default language, but this can be overridden.
        /// Makes an off-box call in out-of-process configuration when the Windows Service is down, and the SQL database is configured.
        /// </remarks>
        public GeoLocationLookupResult LookupGeoLocationByHttpRequest(HttpRequest httpRequest)
        {
            if (httpRequest == null)
                return null;
            return LookupGeoLocationByHttpRequest(httpRequest, GetHttpRequestLanguage(httpRequest)); 
        }

        public GeoLocationLookupResult LookupGeoLocationByHttpRequest(HttpRequest httpRequest, GeoPostalCodeRangeMode postalCodeMode)
        {
            if (httpRequest == null)
                return null;
            return LookupGeoLocationByHttpRequest(httpRequest, GetHttpRequestLanguage(httpRequest),  postalCodeMode);
        }

        public GeoLocationLookupResult LookupGeoLocationByHttpRequest(HttpRequest httpRequest, string language)
        {
            if (httpRequest == null)
                return null;
            return LookupGeoLocationByHttpRequest(httpRequest, language, GeoPostalCodeRangeMode.None);
        }

        public GeoLocationLookupResult LookupGeoLocationByHttpRequest(HttpRequest httpRequest, string language, GeoPostalCodeRangeMode postalCodeMode)
        {
            if (httpRequest == null)
                return null;
            string statsData = httpRequest.RawUrl;
            int mapPointId;

            // Get MappointID from query string
            string mapPointIdString = httpRequest.QueryString.Get(MapPointIdQueryStringName);

            // MappointID from A cookie/Geo value
            if (String.IsNullOrEmpty(mapPointIdString))
            {
                HttpCookie anonymousCookie = httpRequest.Cookies[AnonymousCookieName];

                // user already has a Anon cookie and user already has a GeoCookie
                if ((anonymousCookie != null) && (anonymousCookie[GeoCookieName] != null))
                {
                    mapPointIdString = anonymousCookie[GeoCookieName];
                }
            }
            else
            {
                // Verify the query string is correct
                if (!int.TryParse(mapPointIdString, out mapPointId) || mapPointId <= 0)
                    return null; // ChrisWo want to return null if the query string is invalid. This is mostly used by site manager
            }

            if (!String.IsNullOrEmpty(mapPointIdString))  // MapPointId read from query string or cookie
            {
                if (int.TryParse(mapPointIdString, out mapPointId) && mapPointId > 0)
                {
                    return LookupGeoLocationByMapPointId(mapPointId, language, postalCodeMode, statsData);
                }
                // no need to log error (will flood eventlog), because this is user error, 
            }

            // Looks for IP address in NetScaler added header
            string ipHeaderKey = NetscalerIPHeader;//"client-ip"; // MSCOMGeoConfig.NetscalerIPHeader; // read header key from config file // VIJAY TODO Check!
            string ipAddress = httpRequest.Headers[ipHeaderKey];

            // No NetScaler-added header
            if (String.IsNullOrEmpty(ipAddress))
            {
                ipAddress = httpRequest.UserHostAddress; //httpRequest.ServerVariables["REMOTE_ADDR"];
            }

            return LookupGeoLocationByIPAddress(ipAddress, language, postalCodeMode, statsData);
           
        }

        //=====================================================================
        //  Method: CovertToGeoInfo
        //
        /// <summary>
        /// This method returns GeoInfo data from GeoLocationLookupResult object
        /// </summary>  
        //=====================================================================

        public GeoInfo CovertToGeoInfo(GeoLocationLookupResult lookupResult)
        {
            GeoInfo geoInfo = null;
            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }
            return geoInfo;
        }


        /// <summary>
        /// Gets the user language from HTTP request.
        /// </summary>
        /// <param name="httpRequest">The HTTP request.</param>
        /// <returns>
        /// The language used by the user at client side.
        /// </returns>
        /// <remarks>
        /// Parsing of language from the http request header (Accept-Language).
        /// NULL value is fine. There is no default value and there is no checking of NULL for language in all other APIs. It will be handled through the call stacks later.
        /// </remarks>
        private string GetHttpRequestLanguage(HttpRequest httpRequest)
        {
            string language = null;
            if (httpRequest != null)
            {
                // The approach of "language = httpRequest.Headers.Get("Accept-Language")" needs parsing of string separated with ',', which makes it the same performance as the recommended approach of using UserLanguages.
                if (httpRequest.UserLanguages != null && httpRequest.UserLanguages.Length > 0)
                {
                    language = httpRequest.UserLanguages[0];
                }
            }
            return language;
        }

        #endregion


        #region LookupChildListFromId
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

            if (this.config.UseServiceCacheFile != false)
            {
                Debug.Assert(this.GeoLocationProxy == null);
                

                if (GeoLocationClient.serviceRefreshProcessor != null)
                {
                    lookupResult = GeoLocationClient.serviceRefreshProcessor.LookupChildListFromId(id, languageId);
                }
                else
                {
                    //
                    //  We always fall back to calling the service, if there
                    //  is something wrong with the cached data
                    //

                    lookupResult = this.LookupChildListFromIdAutonomous(id, languageId);
                }
            }
            else
            {
                Debug.Assert(this.GeoLocationProxy != null);
                

                lookupResult = this.GeoLocationProxy.LookupChildListFromId(id, languageId);
            }

            return lookupResult;
        }


        //=====================================================================
        //  Method: LookupChildListFromIdAutonomous
        //
        /// <summary>
        /// This method returns Country Region List
        /// </summary>  
        //=====================================================================

        private List<GeoEntityInfo> LookupChildListFromIdAutonomous(int id, int languageId)
        {
            GeoLocationProxy locationProxy = null;

            List<GeoEntityInfo> lookupResult = null;

            try
            {
                locationProxy = GeoLocationClient.CreateGeoLocationProxy(
                    this.config
                    );


                if (locationProxy != null)
                {
                    lookupResult = locationProxy.LookupChildListFromId(id, languageId);
                }
            }
            finally
            {
                if (locationProxy != null)
                {
                    GeoLocationClient.CloseGeoLocationProxy(
                        locationProxy
                        );
                }
            }

            return lookupResult;
        }

        //=====================================================================
        //  Method: LookupChildListFromId with out locale id
        //
        /// <summary>
        /// This method returns child list from id 
        /// </summary>  
        //=====================================================================
        public List<GeoEntityInfo> LookupChildListFromId(int id)
        {
            //Default locale id for "en" is 9
            return LookupChildListFromId(id, 9);
        }

        #endregion


        #region Geo 2.0 Methods

        //=====================================================================
        //  Method: GetGeoInfoFromIPAddress
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified IP address
        /// </summary>  
        //=====================================================================

        public GeoInfo GetGeoInfoFromIPAddress(
            string ipAddress,
            string language,
            string appNameOrRequestedUrl
            )
        {
            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationByIPAddress(ipAddress, language, appNameOrRequestedUrl);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromIPAddress with postal code mode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified IP address
        /// </summary>  
        //=====================================================================

        public GeoInfo GetGeoInfoFromIPAddress(
            string ipAddress,
            string language,
            GeoPostalCodeRangeMode postalCodeMode,
            string appNameOrRequestedUrl
            )
        {
            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationByIPAddress(ipAddress, language, postalCodeMode, appNameOrRequestedUrl);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromMapPointId
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified MapPoint Id
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromMapPointId(
            int mapPointId,
            string language,
            string appNameOrRequestedUrl
            )
        {
            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationByMapPointId(mapPointId, language, appNameOrRequestedUrl);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromMapPointId with Postalcode Mode
        //
        /// <summary>
        /// This method returns GeoInfo data based on the specified MapPoint Id
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromMapPointId(
            int mapPointId,
            string language,
            GeoPostalCodeRangeMode postalCodeMode, 
            string appNameOrRequestedUrl
            )
        {
            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationByMapPointId(mapPointId, language, postalCodeMode, appNameOrRequestedUrl);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromHttpRequest
        //
        /// <summary>
        /// This method returns GeoInfo data for the given HttpRequest
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromHttpRequest(HttpRequest httpRequest)
        {
             if (httpRequest == null)
                return null;

            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult =  LookupGeoLocationByHttpRequest(httpRequest, GetHttpRequestLanguage(httpRequest)); 

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromHttpRequest with language
        //
        /// <summary>
        /// This method returns GeoInfo data for the given HttpRequest & language
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromHttpRequest(HttpRequest httpRequest, string language)
        {
            if (httpRequest == null)
                return null;

            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationByHttpRequest(httpRequest, language);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromHttpRequest with postal code
        //
        /// <summary>
        /// This method returns GeoInfo data for the given HttpRequest & language
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromHttpRequest(HttpRequest httpRequest, GeoPostalCodeRangeMode postalCodeMode)
        {
            if (httpRequest == null)
                return null;

            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationByHttpRequest(httpRequest, postalCodeMode);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromHttpRequest with Postal code mode and language
        //
        /// <summary>
        /// This method returns GeoInfo data for the given HttpRequest & language
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromHttpRequest(HttpRequest httpRequest, string language, GeoPostalCodeRangeMode postalCodeMode)
        {
            if (httpRequest == null)
                return null;

            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationByHttpRequest(httpRequest, language, postalCodeMode);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromPostalCode with language
        //
        /// <summary>
        /// This method returns GeoInfo data for the given postal code and country
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromPostalCode(string postalCode, string countryCode, string language, string appNameOrRequestedUrl)
        {
            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationFromPostalCode(postalCode, countryCode, language, appNameOrRequestedUrl);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }

        //=====================================================================
        //  Method: GetGeoInfoFromPostalCode with Postal code mode and language
        //
        /// <summary>
        /// This method returns GeoInfo data for the given postal code and country
        /// </summary>  
        //=====================================================================
        public GeoInfo GetGeoInfoFromPostalCode(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalCodeMode,string appNameOrRequestedUrl)
        {
            GeoInfo geoInfo = null;
            GeoLocationLookupResult lookupResult = LookupGeoLocationFromPostalCode(postalCode, countryCode, language, postalCodeMode, appNameOrRequestedUrl);

            if (lookupResult != null && lookupResult.GeoLocationInfo != null)
            {
                geoInfo = PopulateGeoInfo(lookupResult.GeoLocationInfo);
            }

            return geoInfo;
        }




        //=====================================================================
        //  Method: PopulateGeoInfo
        //
        /// <summary>
        /// This method maps the data from GeoLocationInfo to GeoInfo 
        /// and returns GeoInfo data 
        /// </summary>  
        //=====================================================================
        private GeoInfo PopulateGeoInfo(GeoLocationInfo locationInfo)
        {
            GeoInfo geoInfo = null;
            if (locationInfo != null)
            {
                geoInfo = new GeoInfo();
                geoInfo.CityDisplayName = locationInfo.CityName;
                geoInfo.CountryRegionCode = locationInfo.CountryRegionCode;
                geoInfo.CountryRegionDisplayName = locationInfo.CountryRegionName;
                geoInfo.DstEndTimeUniversal = locationInfo.DstEndTimeUniversal;
                geoInfo.DstStartTimeUniversal = locationInfo.DstStartTimeUniversal;
                geoInfo.DstTimeZoneDisplayName = locationInfo.DstTimeZoneName;
                //geoInfo.DstTimeZoneOffset = ????
                geoInfo.DstTimeZoneOffsetDouble = locationInfo.DstTimeZoneOffsetDouble;
                geoInfo.IsEuropeanUnion = locationInfo.IsEuropeanUnion;
                geoInfo.Latitude = locationInfo.Latitude;
                geoInfo.Longitude = locationInfo.Longitude;
                geoInfo.MapPointId = locationInfo.MapPointId;
                geoInfo.MapPointIdParent = locationInfo.MapPointIdParent;
                geoInfo.PostalCodeInfo = locationInfo.PostalCodeInfo;
                //geoInfo.PostalCodes = ???
                //geoInfo.StandardTimeZoneOffset = ???
                geoInfo.StandardTimeZoneOffsetDouble = locationInfo.StandardTimeZoneOffsetDouble;
                geoInfo.StateProvinceCode = locationInfo.StateProvinceCode;
                geoInfo.StateProvinceDisplayName = locationInfo.StateProvinceName;
                geoInfo.TimeZoneDisplayName = locationInfo.TimeZoneName;

            }
            return geoInfo;
        }

        #endregion

        #endregion



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
            Debug.Assert(this.GeoLocationProxy != null);

            GeoLocationDataVersion geoLocationVersion = null;

            if (this.config.UseServiceCacheFile != false)
            {
                if (GeoLocationClient.serviceRefreshProcessor != null)
                {
                    geoLocationVersion = GeoLocationClient.serviceRefreshProcessor.CurrentVersion;
                }
                else
                {
                    //
                    //  fallback to the service if no data available
                    //  in the in-memory cache
                    //

                    geoLocationVersion = this.RetrieveCurrentVersionAutonomous();
                }
            }
            else
            {
                geoLocationVersion = this.GeoLocationProxy.RetrieveCurrentVersion();
            }

            return geoLocationVersion;
        }

        //=====================================================================
        //  Method: RetrieveCurrentVersionAutonomous
        //
        /// <summary>
        /// This method returns version information for the current Geo data
        /// served by the service
        /// </summary>  
        //=====================================================================

        public GeoLocationDataVersion RetrieveCurrentVersionAutonomous()
        {
            GeoLocationProxy locationProxy = null;

            GeoLocationDataVersion geoLocationVersion = null;

            try
            {
                locationProxy = GeoLocationClient.CreateGeoLocationProxy(
                    this.config
                    );

                if (locationProxy != null)
                {
                    geoLocationVersion = locationProxy.RetrieveCurrentVersion();
                }
            }
            finally
            {
                if (locationProxy != null)
                {
                    GeoLocationClient.CloseGeoLocationProxy(
                        locationProxy
                        );
                }
            }

            return geoLocationVersion;
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
            string filePath = null;

            if (this.config.UseServiceCacheFile != false)
            {
                Debug.Assert(this.GeoLocationProxy == null);
                

                if (GeoLocationClient.serviceRefreshProcessor != null)
                {
                    filePath = GeoLocationClient.serviceRefreshProcessor.CurrentCacheFilePath;
                }
                else
                {
                    filePath = this.RetrieveCacheDataFileAutonomous();
                }
            }
            else
            {
                Debug.Assert(this.GeoLocationProxy != null);
                

                filePath = this.GeoLocationProxy.RetrieveCacheDataFile();
            }

            return filePath;
        }

        //=====================================================================
        //  Method: RetrieveCacheDataFile
        //
        /// <summary>
        /// This method returns cach geo data data file to use
        /// </summary>  
        //=====================================================================

        private string RetrieveCacheDataFileAutonomous()
        {
            GeoLocationProxy locationProxy = null;

            string filePath = null;

            try
            {
                locationProxy = GeoLocationClient.CreateGeoLocationProxy(
                    this.config
                    );

                if (locationProxy != null)
                {
                    filePath = locationProxy.RetrieveCacheDataFile();
                }
            }
            finally
            {
                if (locationProxy != null)
                {
                    GeoLocationClient.CloseGeoLocationProxy(
                        locationProxy
                        );
                }
            }

            return filePath;
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
            Debug.Assert(this.GeoLocationProxy != null);

            GeoLocationRefreshInfo refreshInfo = null;

            if (this.config.UseServiceCacheFile == false)
            {
                refreshInfo = this.GeoLocationProxy.RetrieveDataRefreshDetail();
            }
            else
            {
                throw new InvalidOperationException(
                    "Cannot use the RetrieveDataRefreshDetail functionality when not connected to a service."
                    );
            }

            return refreshInfo;


        }

        //=====================================================================
        //  Method: Open
        //
        /// <summary>
        /// This method opens the connection to the geo services
        /// </summary>  
        //=====================================================================

        public void Open()
        {
            if (this.GeoLocationProxy != null)
            {
                if (this.GeoLocationProxy.State == CommunicationState.Closed)
                {
                    this.GeoLocationProxy.Open();
                }
            }
        }

        //=====================================================================
        //  Method: Open
        //
        /// <summary>
        /// This method opens the connection to the geo services
        /// </summary>  
        //=====================================================================

        public void Close()
        {
            this.CloseGeoLocationProxy();

            // TraceLogging.Current.Stop();
        }

        //=====================================================================
        //  Method: Initialize
        //
        /// <summary>
        /// Initialization actions for the service
        /// </summary>  
        //=====================================================================

        public void Initialize()
        {
            GeoRefreshConfiguration refreshConfiguration = null;

            if (this.config.UseServiceCacheFile != false)
            {
                if (this.config.ClientAutoRefresh != false)
                {
                    refreshConfiguration = GeoRefreshConfiguration.CreateConfiguration(
                        this.config.ClientRefreshScheduledUtcTime,
                        this.config.ClientRefreshTimeInterval
                        );
                }

                FromServiceGeoRefreshProcessor refreshProcessor = FromServiceGeoRefreshProcessor.Create(
                    null,
                    refreshConfiguration,
                    this.config
                    );

                refreshProcessor.Start();

                if (this.config.ReadServiceCacheFile)
                {
                    refreshProcessor.CurrentCacheFilePath = this.config.ServiceCacheFilePath;
                }
                
                GeoLocationClient.serviceRefreshProcessor = refreshProcessor;


            }
        }

        private void CloseGeoLocationProxy()
        {
            if (this.GeoLocationProxy != null)
            {
                GeoLocationClient.CloseGeoLocationProxy(
                    this.GeoLocationProxy
                    );
            }
        }

        private static void CloseGeoLocationProxy(
            GeoLocationProxy locationProxy
            )
        {
            if (locationProxy != null)
            {
                if (locationProxy.State == CommunicationState.Opened)
                {
                    //
                    //  this call could fail, as the channel might be timed out
                    //  so we catch the communication exception and abort
                    //  if that happens
                    //

                    try
                    {
                        locationProxy.Close();
                    }
                    catch (CommunicationException)
                    {
                        locationProxy.Abort();
                    }
                }
                else if (locationProxy.State == CommunicationState.Faulted)
                {
                    locationProxy.Abort();
                }
            }
        }


        private static GeoLocationProxy CreateGeoLocationProxy(
            GeoConfiguration configuration
            )
        {
            GeoLocationProxy locationProxy = null;
            

            //
            //  Initialize the client certificate property
            //

            if (configuration.IsSecure != false)
            {
                string clientCertificateThumbprint = null;

                //
                //  Get the thumbprint
                //

                clientCertificateThumbprint = configuration.Certificate;

                if (string.IsNullOrEmpty(clientCertificateThumbprint) == false)
                {
                    locationProxy.ClientCredentials.ClientCertificate.SetCertificate(
                        StoreLocation.LocalMachine,
                        StoreName.My,
                        X509FindType.FindByThumbprint,
                        clientCertificateThumbprint
                        );
                }
            }

            return locationProxy;
        }

      
        
    }
}
