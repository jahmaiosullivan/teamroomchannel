using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace Microsoft.IT.Geo.Client
{
    //=========================================================================
    //  Class: GeoConfiguration
    /// <summary>
    /// This class contains app.config settings for Geo
    /// </summary>
    //=========================================================================

    public sealed class GeoConfiguration
    {
        private const string HostNameKey = "GeoServerName";
        private const string HostPortKey = "GeoPortNumber";
        private const string SecuredServerKey = "IsGeoServerSecured";
        private const string ClientCertificate = "GeoClientCertificate";
        private const string EndpointAddressKey = "GeoEndpointAddress";
        private const string TransportTypeKey = "GeoTransportType";
        private const string ClientSendTimeoutKey = "GeoClientSendTimeout";

        private const string UseServiceCacheFileKey = "GeoClientUseServiceCacheFile";

        private const string ReadServiceCacheFileOnlyKey = "GeoClientReadServiceCacheFile";

        private const string GeoServiceAutoRefreshKey = "GeoService.AutoRefresh";

        private const string GeoServiceRefreshScheduledUtcTimeKey = "GeoService.RefreshScheduledUtcTime";
        private const string GeoServiceRefreshIntervalKey = "GeoService.RefreshInterval";

        private const string GeoClientAutoRefreshKey = "GeoClient.AutoRefresh";

        private const string GeoClientRefreshScheduledUtcTimeKey = "GeoClient.RefreshScheduledUtcTime";
        private const string GeoClientRefreshIntervalKey = "GeoClient.RefreshInterval";

        private const string EventLogNameKey = "EventLogName";
        private const string EventSourceNameKey = "EventSourceName";

        //=====================================================================
        //  Method: GeoConfiguration
        //
        /// <summary>
        /// This is the class default constructor.
        /// </summary>  
        //=====================================================================

        private GeoConfiguration()
            : base()
        {
            this.HostName = GeoConfiguration.GetHostName();
            this.HostPort = GeoConfiguration.GetHostPort();
            this.TransportType = GeoConfiguration.GetTransportType();
            this.IsSecure = GeoConfiguration.GetIsSecured();
            this.Certificate = GeoConfiguration.GetClientCertificateThumbprint();
            this.EndpointAddress = GeoConfiguration.GetEndpointAddress();
            this.ServerUri = GeoConfiguration.GetServerUri();
            this.ClientSendTimeout = GeoConfiguration.GetClientSendTimeout();

            this.EventLogName = GeoConfiguration.GetEventLogName();
            this.EventSourceName = GeoConfiguration.GetEventSourceName();

            this.UseServiceCacheFile = GeoConfiguration.ReadBoolConfig(
                GeoConfiguration.UseServiceCacheFileKey,
                GeoConfiguration.DefaultUseServiceCacheFile
                );

            this.ReadServiceCacheFile = GeoConfiguration.ReadBoolConfig(
               GeoConfiguration.ReadServiceCacheFileOnlyKey,
               GeoConfiguration.DefaultReadServiceCacheFile
               );

            this.ServiceAutoRefresh = GeoConfiguration.ReadBoolConfig(
                GeoConfiguration.GeoServiceAutoRefreshKey,
                GeoConfiguration.DefaultServiceAutoRefresh
                );

            this.ServiceRefreshScheduledUtcTime = GeoConfiguration.ReadUtcTimeConfig(
                GeoConfiguration.GeoServiceRefreshScheduledUtcTimeKey,
                GeoConfiguration.DefaultServiceRefreshUtcTime
                );

            this.ServiceRefreshTimeInterval = GeoConfiguration.ReadTimeIntervalConfig(
                GeoConfiguration.GeoServiceRefreshIntervalKey,
                GeoConfiguration.DefaultServiceRefreshInterval
                );


            this.ClientAutoRefresh = GeoConfiguration.ReadBoolConfig(
                GeoConfiguration.GeoClientAutoRefreshKey,
                GeoConfiguration.DefaultClientAutoRefresh
                );

            this.ClientRefreshScheduledUtcTime = GeoConfiguration.ReadUtcTimeConfig(
                GeoConfiguration.GeoClientRefreshScheduledUtcTimeKey,
                GeoConfiguration.DefaultClientRefreshUtcTime
                );

            this.ClientRefreshTimeInterval = GeoConfiguration.ReadTimeIntervalConfig(
                GeoConfiguration.GeoClientRefreshIntervalKey,
                GeoConfiguration.DefaultClientRefreshInterval
                );
        }

        //=====================================================================
        //  Method: GeoConfiguration
        //
        /// <summary>
        /// This is the class default constructor.
        /// </summary>  
        //=====================================================================

        private GeoConfiguration(
            Uri serverUri
            )
            : base()
        {
            if (serverUri != null)
            {
                if (serverUri.IsAbsoluteUri != false)
                {
                    Debug.Assert(serverUri.IsAbsoluteUri == true);

                    this.HostName = serverUri.Host;
                    this.HostPort = serverUri.Port;
                    this.IsSecure = false;
                    this.Certificate = null;
                    this.ServerUri = serverUri;
                    this.ClientSendTimeout = GeoConfiguration.GetClientSendTimeout();

                    this.EndpointAddress = serverUri.AbsolutePath;

                    // do not include the first "/" in the EndpointAddress
                    
                    if (serverUri.AbsolutePath[0] == '/')
                    {
                        this.EndpointAddress = serverUri.AbsolutePath.Substring(1);
                    }

                    int result = string.Compare(
                        serverUri.Scheme,
                        Uri.UriSchemeHttps,
                        StringComparison.OrdinalIgnoreCase
                        );

                    if (result == 0)
                    {
                        this.IsSecure = true;
                        this.Certificate = GeoConfiguration.GetClientCertificateThumbprint();
                    }
                }
                else
                {
                    throw new ArgumentException(
                        "The server uri needs to be an absolute URI, not a relative one.",
                        "serverUri"
                        );
                }
            }
            else
            {
                throw new ArgumentNullException(
                    "serverUri",
                    "ServerUri specified is null."
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

        public static GeoConfiguration CreateConfiguration()
        {
            GeoConfiguration configuration = new GeoConfiguration();

            return configuration;
        }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This is the class default constructor.
        /// </summary>  
        //=====================================================================

        public static GeoConfiguration CreateConfiguration(
            Uri serverUri
            )
        {
            GeoConfiguration identityConfiguration = null;

            identityConfiguration = new GeoConfiguration(
                serverUri
                );

            return identityConfiguration;
        }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This method returns the default configuration
        /// </summary>  
        //=====================================================================

        public static GeoConfiguration CreateConfiguration(
            string hostName
            )
        {
            int hostPort = GeoConfiguration.GetHostPort();

            return GeoConfiguration.CreateConfiguration(
                hostName,
                hostPort
                );
        }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This method returns the default configuration
        /// </summary>  
        //=====================================================================

        public static GeoConfiguration CreateConfiguration(
            string hostName,
            int hostPort
            )
        {
            string endpointAddress = GeoConfiguration.GetEndpointAddress();

            return GeoConfiguration.CreateConfiguration(
                hostName,
                hostPort,
                endpointAddress
                );
        }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This method returns the default configuration
        /// </summary>  
        //=====================================================================

        public static GeoConfiguration CreateConfiguration(
            string hostName,
            int hostPort,
            string endpointAddress
            )
        {
            string transportType = GeoConfiguration.GetTransportType();

            bool isSecure = GeoConfiguration.GetIsSecured();

            return GeoConfiguration.CreateConfiguration(
                hostName,
                hostPort,
                endpointAddress,
                transportType,
                isSecure
                );
        }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This method returns the default configuration
        /// </summary>  
        //=====================================================================

        public static GeoConfiguration CreateConfiguration(
            string hostName,
            int hostPort,
            string endpointAddress,
            string transportType,
            bool secure
            )
        {
            GeoConfiguration configuration = new GeoConfiguration();

            configuration.HostName = hostName;
            configuration.HostPort = hostPort;
            configuration.IsSecure = secure;
            configuration.Certificate = GeoConfiguration.GetClientCertificateThumbprint();
            configuration.EndpointAddress = endpointAddress;
            configuration.TransportType = transportType;

            configuration.ServerUri = GeoConfiguration.GetServerUri(
                hostName,
                hostPort,
                transportType,
                secure,
                configuration.EndpointAddress
                );

            return configuration;
        }

        //=====================================================================
        //  Method: DefaultMethodName
        //
        /// <summary>
        /// This method returns the default Membership Server host name
        /// </summary>  
        //=====================================================================

        public static string DefaultMethodName
        {
            get
            {
                return "net.tcp";
            }
        }

        //=====================================================================
        //  Method: DefaultHostName
        //
        /// <summary>
        /// This method returns the default Geo Server host name
        /// </summary>  
        //=====================================================================

        public static string DefaultHostName
        {
            get
            {
                return Environment.MachineName;
            }
        }

        //=====================================================================
        //  Method: DefaultHostPort
        //
        /// <summary>
        /// This method returns the default Geo Server host port
        /// </summary>  
        //=====================================================================

        public static int DefaultHostPort
        {
            get
            {
                return 9920;
            }
        }

        //=====================================================================
        //  Method: DefaultEndpoint
        //
        /// <summary>
        /// This method returns the default Geo Server end point
        /// </summary>  
        //=====================================================================

        public static string DefaultEndpoint
        {
            get
            {
                return "GeoServer";
            }
        }

        //=====================================================================
        //  Method: DefaultClientSendTimeout
        //
        /// <summary>
        /// This method returns the default Geo Server client send timeout
        /// </summary>  
        //=====================================================================

        public static int DefaultClientSendTimeout
        {
            get
            {
                return 10000;
            }
        }

        //=====================================================================
        //  Method: DefaultAddress
        //
        /// <summary>
        /// This method returns the default Geo Server URI.
        /// </summary>  
        //=====================================================================

        public static string DefaultAddress
        {
            get
            {
                StringBuilder address = new StringBuilder("net.tcp://");

                address.AppendFormat(
                    "{0}:{1}/{2}",
                    GeoConfiguration.DefaultHostName,
                    GeoConfiguration.DefaultHostPort,
                    GeoConfiguration.DefaultEndpoint
                    );

                return address.ToString();
            }
        }

        //=====================================================================
        //  Method: DefaultSecureAddress
        //
        /// <summary>
        /// This method returns the default Geo Server URI.
        /// </summary>  
        //=====================================================================

        public static string DefaultSecureAddress
        {
            get
            {
                StringBuilder address = new StringBuilder("net.tcp://");

                address.AppendFormat(
                    "{0}:{1}/{2}",
                    GeoConfiguration.DefaultHostName,
                    GeoConfiguration.DefaultHostPort,
                    GeoConfiguration.DefaultEndpoint
                    );

                return address.ToString();
            }
        }

        //=====================================================================
        //  Method: DefaultUseServiceCacheFile
        //
        /// <summary>
        /// This method returns the default value for UserServiceCacheFile
        /// config
        /// </summary>  
        //=====================================================================

        private static bool DefaultUseServiceCacheFile
        {
            get
            {
                return false;
            }
        }

        //=====================================================================
        //  Method: DefaultReadServiceCacheFile
        //
        /// <summary>
        /// This method returns the default value for ReadServiceCacheFile
        /// config
        /// </summary>  
        //=====================================================================

        private static bool DefaultReadServiceCacheFile
        {
            get
            {
                return false;
            }
        }

        //=====================================================================
        //  Method: DefaultServiceAutoRefresh
        //
        /// <summary>
        /// This method returns the default value for 
        /// ServiceAutoRefresh config
        /// </summary>  
        //=====================================================================

        private static bool DefaultServiceAutoRefresh
        {
            get
            {
                return false;
            }
        }

        private static TimeSpan DefaultServiceRefreshInterval = new TimeSpan(24, 0, 0);

        private static DateTime DefaultServiceRefreshUtcTime
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
        //  Method: DefaultServiceAutoRefresh
        //
        /// <summary>
        /// This method returns the default value for 
        /// ServiceAutoRefresh config
        /// </summary>  
        //=====================================================================

        private static bool DefaultClientAutoRefresh
        {
            get
            {
                return false;
            }
        }

        private static TimeSpan DefaultClientRefreshInterval = new TimeSpan(24, 0, 0);

        private static DateTime DefaultClientRefreshUtcTime
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
        //  Method: DefaultEventLogName
        //
        /// <summary>
        /// This method returns the default Geo Service event log
        /// </summary>  
        //=====================================================================

        public static string DefaultEventLogName
        {
            get
            {
                return "GeoServices";
            }
        }

        //=====================================================================
        //  Method: DefaultEventSourceName
        //
        /// <summary>
        /// This method returns the default Geo Service event source
        /// </summary>  
        //=====================================================================

        public static string DefaultEventSourceName
        {
            get
            {
                return "GeoServiceEvent";
            }
        }

        //=====================================================================
        //  Method: HostName
        //
        /// <summary>
        /// This property returns the host server name
        /// </summary>  
        //=====================================================================

        public string HostName { get; private set; }

        //=====================================================================
        //  Method: HostPort
        //
        /// <summary>
        /// This property returns the host server name
        /// </summary>  
        //=====================================================================

        public int HostPort { get; private set; }

        //=====================================================================
        //  Method: TransportType
        //
        /// <summary>
        /// This property returns the transport type name
        /// </summary>  
        //=====================================================================

        public string TransportType { get; private set; }

        //=====================================================================
        //  Method: IsSecure
        //
        /// <summary>
        /// This property returns true if the connection is secured
        /// </summary>  
        //=====================================================================

        public bool IsSecure { get; private set; }

        //=====================================================================
        //  Method: Certificate
        //
        /// <summary>
        /// This property returns the client certification for connecting over
        /// TLS.
        /// </summary>  
        //=====================================================================

        public string Certificate { get; private set; }

        //=====================================================================
        //  Method: EndpointAddress
        //
        /// <summary>
        /// This property returns the endpoint address
        /// </summary>  
        //=====================================================================
        public string EndpointAddress { get; private set; }


        //=====================================================================
        //  Method: ClientSendTimeout
        //
        /// <summary>
        /// This property returns the binding.SendTimeout for the client side
        /// </summary>  
        //=====================================================================

        public int ClientSendTimeout { get; set; }

        //=====================================================================
        //  Method: ServerUri
        //
        /// <summary>
        /// This property returns the server URI
        /// </summary>  
        //=====================================================================

        public Uri ServerUri { get; private set; }


        //=====================================================================
        //  Method: EventLogName
        //
        /// <summary>
        /// This property returns the event log name
        /// </summary>  
        //=====================================================================

        public string EventLogName { get; set; }

        //=====================================================================
        //  Method: EventSourceName
        //
        /// <summary>
        /// This property returns the event source name
        /// </summary>  
        //=====================================================================

        public string EventSourceName { get; set; }

        //=====================================================================
        //  Method: IsHttp
        //
        /// <summary>
        /// This property returns true if the TransportType is Http
        /// </summary>  
        //=====================================================================

        public bool IsHttp
        {
            get
            {
                return string.Compare(
                    "http",
                    this.TransportType,
                    StringComparison.OrdinalIgnoreCase
                    ) == 0;
            }
        }

        //=====================================================================
        //  Method: IsHttps
        //
        /// <summary>
        /// This property returns true if the TransportType is Https
        /// </summary>  
        //=====================================================================

        public bool IsHttps
        {
            get
            {
                return string.Compare(
                    "https",
                    this.TransportType,
                    StringComparison.OrdinalIgnoreCase
                    ) == 0;
            }
        }

        //=====================================================================
        //  Method: IsNetTcp
        //
        /// <summary>
        /// This property returns true if the TransportType is net.tcp
        /// </summary>  
        //=====================================================================

        public bool IsNetTcp
        {
            get
            {
                return string.Compare(
                    "net.tcp",
                    this.TransportType,
                    StringComparison.OrdinalIgnoreCase
                    ) == 0;
            }
        }

        //=====================================================================
        //  Method: UseServiceCacheFile
        //
        /// <summary>
        /// This property returns true if the client should use the cache file
        /// whose file path is retrieved from the service
        /// </summary>  
        //=====================================================================

        public bool UseServiceCacheFile { get; set; }

        //=====================================================================
        //  Method: ReadServiceCacheFile
        //
        /// <summary>
        /// This property returns true if the client should Read the cache file
        /// specified in the connect method instead of retrieve from the service
        /// </summary>  
        //=====================================================================

        public bool ReadServiceCacheFile { get; set; }

        //=====================================================================
        //  Method: ServiceCacheFilePath
        //
        /// <summary>
        /// This property returns the cache file path proviced in connect method 
        /// </summary>  
        //=====================================================================

        public string ServiceCacheFilePath { get; set; }

        //=====================================================================
        //  Method: ServiceCacheFolderPath
        //
        /// <summary>
        /// This property returns the cache Folder path proviced in connect method 
        /// </summary>  
        //=====================================================================

        public string ServiceCacheFolderPath { get; set; }

        //=====================================================================
        //  Method: AutoRefresh
        //
        /// <summary>
        /// Controls weather or not the Geo Service is refreshing the in-memory
        /// geo data automatically
        /// </summary>  
        //=====================================================================

        public bool ServiceAutoRefresh { get; private set; }

        //=====================================================================
        //  Method: ServiceRefreshScheduledUtcTime
        //
        /// <summary>
        /// This method returns the value for 
        /// ServiceRefreshScheduledUtcTime config
        /// </summary>  
        //=====================================================================

        public DateTime ServiceRefreshScheduledUtcTime { get; private set; }

        //=====================================================================
        //  Method: ServiceRefreshTimeInterval
        //
        /// <summary>
        /// This method returns the value for 
        /// ServiceRefreshTimeInterval config
        /// </summary>  
        //=====================================================================

        public TimeSpan ServiceRefreshTimeInterval { get; private set; }

        //=====================================================================
        //  Method: AutoRefresh
        //
        /// <summary>
        /// Controls weather or not the Geo Service is refreshing the in-memory
        /// geo data automatically
        /// </summary>  
        //=====================================================================

        public bool ClientAutoRefresh { get; set; }

        //=====================================================================
        //  Method: ServiceRefreshScheduledUtcTime
        //
        /// <summary>
        /// This method returns the value for 
        /// ServiceRefreshScheduledUtcTime config
        /// </summary>  
        //=====================================================================

        public DateTime ClientRefreshScheduledUtcTime { get; set; }

        //=====================================================================
        //  Method: ServiceRefreshTimeInterval
        //
        /// <summary>
        /// This method returns the value for 
        /// ServiceRefreshTimeInterval config
        /// </summary>  
        //=====================================================================

        public TimeSpan ClientRefreshTimeInterval { get; set; }

        //=====================================================================
        //  Method: ServiceRefreshScheduledUtcTime
        //
        /// <summary>
        /// This method returns the value for 
        /// ServiceRefreshScheduledUtcTime config
        /// </summary>  
        //=====================================================================

        public GeoConfiguration Clone()
        {
            GeoConfiguration clonedConfiguration = new GeoConfiguration();

            clonedConfiguration.Certificate = this.Certificate;
            clonedConfiguration.ClientAutoRefresh = this.ClientAutoRefresh;
            clonedConfiguration.ClientRefreshScheduledUtcTime = this.ClientRefreshScheduledUtcTime;
            clonedConfiguration.ClientRefreshTimeInterval = this.ClientRefreshTimeInterval;
            clonedConfiguration.ClientSendTimeout = this.ClientSendTimeout;
            clonedConfiguration.EndpointAddress = this.EndpointAddress;
            clonedConfiguration.HostName = this.HostName;
            clonedConfiguration.HostPort = this.HostPort;

            clonedConfiguration.IsSecure = this.IsSecure;
            clonedConfiguration.ServerUri = this.ServerUri;
            clonedConfiguration.ServiceAutoRefresh = this.ServiceAutoRefresh;
            clonedConfiguration.ServiceRefreshScheduledUtcTime = this.ServiceRefreshScheduledUtcTime;
            clonedConfiguration.ServiceRefreshTimeInterval = this.ServiceRefreshTimeInterval;
            clonedConfiguration.TransportType = this.TransportType;
            clonedConfiguration.UseServiceCacheFile = this.UseServiceCacheFile;
            clonedConfiguration.ReadServiceCacheFile = this.ReadServiceCacheFile;
            clonedConfiguration.ServiceCacheFolderPath = this.ServiceCacheFolderPath;
            clonedConfiguration.ServiceCacheFilePath = this.ServiceCacheFilePath;

            clonedConfiguration.EventLogName = this.EventLogName;
            clonedConfiguration.EventSourceName = this.EventSourceName;

            return clonedConfiguration;
        }

        //=====================================================================
        //  Method: GetEventLog
        //
        /// <summary>
        /// This method returns the Geo Services event log name.
        /// </summary>  
        //=====================================================================

        public static string GetEventLogName()
        {
            //
            //  Get the config value
            //

            string configuredValue = ConfigurationManager.AppSettings[GeoConfiguration.EventLogNameKey];

            if (string.IsNullOrEmpty(configuredValue) != false)
            {
                return GeoConfiguration.DefaultEventLogName;
            }

            return configuredValue;
        }

        //=====================================================================
        //  Method: GetEventSourceName
        //
        /// <summary>
        /// This method returns the Geo Services event source name.
        /// </summary>  
        //=====================================================================

        public static string GetEventSourceName()
        {
            //
            //  Get the config value
            //

            string configuredValue = ConfigurationManager.AppSettings[GeoConfiguration.EventSourceNameKey];

            if (string.IsNullOrEmpty(configuredValue) != false)
            {
                return GeoConfiguration.DefaultEventSourceName;
            }

            return configuredValue;

        }

        //=====================================================================
        //  Method: GetTransportType
        //
        /// <summary>
        /// This method returns the transport method info from configuration,
        /// which can be http, tcp, msmq.
        /// If not  found, it will return the static default.
        /// </summary>  
        //=====================================================================

        private static string GetTransportType()
        {
            //
            //  Get the config value
            //

            string configuredValue = ConfigurationManager.AppSettings[GeoConfiguration.TransportTypeKey];

            if (string.IsNullOrEmpty(configuredValue) != false)
            {
                return GeoConfiguration.DefaultMethodName;
            }

            return configuredValue;
        }

        //=====================================================================
        //  Method: GetHostName
        //
        /// <summary>
        /// This method returns the host name from configuration. If not 
        /// found, it will return the static default.
        /// </summary>  
        //=====================================================================

        private static string GetHostName()
        {
            //
            //  Get the host name
            //

            string hostName = ConfigurationManager.AppSettings[GeoConfiguration.HostNameKey];

            if (string.IsNullOrEmpty(hostName) != false)
            {
                return GeoConfiguration.DefaultHostName;
            }

            if (hostName == ".")
            {
                return GeoConfiguration.DefaultHostName;
            }

            int result = string.Compare(
                hostName,
                "localhost",
                StringComparison.OrdinalIgnoreCase
                );

            if (result == 0)
            {
                return GeoConfiguration.DefaultHostName;
            }

            return hostName;
        }

        //=====================================================================
        //  Method: GetHostPort
        //
        /// <summary>
        /// This method returns the host port from configuration. If not 
        /// found, it will return the static default.
        /// </summary>  
        //=====================================================================

        private static int GetHostPort()
        {
            int hostPort = GeoConfiguration.DefaultHostPort;

            string objectStorePortNumber = ConfigurationManager.AppSettings[GeoConfiguration.HostPortKey];

            if (string.IsNullOrEmpty(objectStorePortNumber) == false)
            {
                bool result = int.TryParse(
                    objectStorePortNumber,
                    out hostPort
                    );

                //
                // If the TryParse failed, rollback to the Default Port
                //

                if (result == false)
                {
                    hostPort = GeoConfiguration.DefaultHostPort;
                }
            }

            return hostPort;
        }

        //=====================================================================
        //  Method: GetHostPort
        //
        /// <summary>
        /// This method returns the host port from configuration. If not 
        /// found, it will return the static default.
        /// </summary>  
        //=====================================================================

        private static int GetClientSendTimeout()
        {
            int sendTimeout = GeoConfiguration.DefaultClientSendTimeout;

            string sendTimeoutValue = ConfigurationManager.AppSettings[GeoConfiguration.ClientSendTimeoutKey];

            if (string.IsNullOrEmpty(sendTimeoutValue) == false)
            {
                bool result = int.TryParse(
                    sendTimeoutValue,
                    out sendTimeout
                    );

                //
                // If the TryParse failed, rollback to the Default Client SendTimeout
                //

                if (result == false)
                {
                    sendTimeout = GeoConfiguration.DefaultClientSendTimeout;
                }
            }

            return sendTimeout;
        }

        //=====================================================================
        //  Method: GetIsSecured
        //
        /// <summary>
        /// This method returns true if a corresponding key is found in the config
        /// file with a value of true, else this method returns a false
        /// </summary>  
        //=====================================================================

        private static bool GetIsSecured()
        {
            bool isSecured = false;

            //
            //  Get the value from config
            //

            string isSecuredValue = ConfigurationManager.AppSettings[GeoConfiguration.SecuredServerKey];

            if (string.IsNullOrEmpty(isSecuredValue) == false)
            {
                bool succeeded = bool.TryParse(
                    isSecuredValue,
                    out isSecured
                    );

                if (succeeded == false)
                {
                    isSecured = false;
                }
            }

            return isSecured;
        }

        //=====================================================================
        //  Method: GetClientCertificateThumbprint
        //
        /// <summary>
        /// This method returns the client certificate thumbprint entry
        /// from the app.config file, or null is none found
        /// </summary>  
        //=====================================================================

        private static string GetClientCertificateThumbprint()
        {
            if (GeoConfiguration.GetIsSecured() != false)
            {
                string clientCertificate = ConfigurationManager.AppSettings[GeoConfiguration.ClientCertificate];

                if (string.IsNullOrEmpty(clientCertificate) == false)
                {
                    return clientCertificate;
                }
                else
                {
                    throw new ArgumentException(
                        "The client certification was not found or secured connections are disabled. Check config file."
                        );
                }
            }
            else
            {
                return null;
            }


        }

        //=====================================================================
        //  Method: GetEndpointAddress
        //
        /// <summary>
        /// This method returns the endpoint addres from configuration. If not 
        /// found, it will return the static default.
        /// </summary>  
        //=====================================================================

        private static string GetEndpointAddress()
        {
            //
            //  Get the endpoint addres
            //

            string endpointAddress = ConfigurationManager.AppSettings[GeoConfiguration.EndpointAddressKey];

            if (string.IsNullOrEmpty(endpointAddress) != false)
            {
                endpointAddress = GeoConfiguration.DefaultEndpoint;
            }

            return endpointAddress;
        }

        //=====================================================================
        //  Method: GetServerUri
        //
        /// <summary>
        /// This method returns the endpoint Uri for the CacheServer service.
        /// It reads the config file for values and if none found it returns
        /// the default address, that is localhost. If values were found then
        /// it builds the Uri from the provided values
        /// </summary>  
        //=====================================================================

        private static Uri GetServerUri()
        {
            return GeoConfiguration.GetServerUri(
                GeoConfiguration.GetHostName(),
                GeoConfiguration.GetHostPort(),
                GeoConfiguration.GetTransportType(),
                GeoConfiguration.GetIsSecured(),
                GeoConfiguration.GetEndpointAddress()
                );
        }

        //=====================================================================
        //  Method: GetServerUri
        //
        /// <summary>
        /// This method returns the endpoint Uri for the CacheServer service.
        /// It reads the config file for values and if none found it returns
        /// the default address, that is localhost. If values were found then
        /// it builds the Uri from the provided values
        /// </summary>  
        //=====================================================================

        private static Uri GetServerUri(
            string hostName,
            int hostPort,
            string transportType,
            bool secure,
            string endpointAddress
            )
        {
            UriBuilder uriBuilder = new UriBuilder();

            //
            //  Build up the URL from the pieces.
            //

            int tcpComparison = string.Compare(
                transportType,
                "net.tcp",
                StringComparison.OrdinalIgnoreCase
                );

            if (tcpComparison == 0)
            {
                uriBuilder.Scheme = "net.tcp";
            }
            else
            {
                uriBuilder.Scheme = (secure == false ? "http" : "https");
            }

            uriBuilder.Host = hostName;
            uriBuilder.Port = hostPort;
            uriBuilder.Path = endpointAddress;

            //
            //  Return the full URI
            //

            return uriBuilder.Uri;
        }

        //=====================================================================
        //  Method: GetUseServiceCacheFile
        //
        /// <summary>
        /// This method returns the configured value, or the default one,
        /// if not present in the app.config, for the UseServiceCacheFile
        /// option
        /// </summary>  
        //=====================================================================

        private static bool ReadBoolConfig(
            string settingName,
            bool defaultValue
            )
        {
            string configValueString = ConfigurationManager.AppSettings[settingName];

            bool configValue = defaultValue;

            if (string.IsNullOrEmpty(configValueString) == false)
            {
                bool isValid = Boolean.TryParse(
                    configValueString,
                    out configValue
                    );

                if (isValid == false)
                {
                    configValue = defaultValue;
                }
            }

            return configValue;
        }

        //=====================================================================
        //  Method: GetGeoDataRefreshUtcTime
        //
        /// <summary>
        /// This method returns the GetGeoDataRefreshUtcTime value from configuration.
        /// </summary>  
        //=====================================================================

        private static DateTime ReadUtcTimeConfig(
            string settingName,
            DateTime defaultValue
            )
        {
            Debug.Assert(defaultValue.Kind == DateTimeKind.Utc);

            DateTime time = defaultValue;

            string configEntry = ConfigurationManager.AppSettings[settingName];

            if (string.IsNullOrEmpty(configEntry) == false)
            {
                bool isValidTime = DateTime.TryParse(
                    configEntry,
                    out time
                    );

                if (isValidTime == false)
                {
                    time = defaultValue;
                }
                else
                {
                    time = new DateTime(
                        DateTime.UtcNow.Year,
                        DateTime.UtcNow.Month,
                        DateTime.UtcNow.Day,
                        time.Hour,
                        time.Minute,
                        time.Second,
                        time.Millisecond,
                        DateTimeKind.Utc
                        );
                }
            }

            return time;
        }

        //=====================================================================
        //  Method: GetGeoDataRefreshInterval
        //
        /// <summary>
        /// This method returns the GetGeoDataRefreshInterval value from configuration.
        /// </summary>  
        //=====================================================================

        private static TimeSpan ReadTimeIntervalConfig(
            string settingName,
            TimeSpan defaultValue
            )
        {
            TimeSpan interval = defaultValue;

            string configEntry = ConfigurationManager.AppSettings[settingName];

            if (string.IsNullOrEmpty(configEntry) == false)
            {
                bool isValidTime = TimeSpan.TryParse(
                    configEntry,
                    out interval
                    );

                if (isValidTime == false)
                {
                    interval = defaultValue;
                }

            }

            return interval;
        }

    }
}
