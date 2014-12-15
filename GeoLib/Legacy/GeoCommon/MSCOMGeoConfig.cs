using System;
using System.Xml;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Globalization;
using Microsoft.IT.Geo.Legacy.GeoCommon;

namespace MSCOMGeoSystem.Common
{
	public static class MSCOMGeoConfig
    {

        private const string IsIPv4EnabledKey = "IsIPv4Enabled";

        private const string IsIPv6EnabledKey = "IsIPv6Enabled";

        static object syncRoot = new object();
        #region fields		
        /// <summary>time of last load of this file</summary>
        private static DateTime lastLoad;
        
        private static DateTime fileLastModified;
        public static DateTime FileLastModified
        {
            get { return fileLastModified; }
        }

        private static FileSystemWatcher watcher;        
        
        /// <summary>the path of GeoSocket.MSCOM.config</summary>
        private static string configPath;
        public static string ConfigPath
        {
            get { return configPath; }
        }

        private static string projectName = "MSCOM/Geo";
        static public string ProjectName
        {
            get { return projectName; }
        }

        static private string configVersion;
        static public string ConfigVersion
        {
            get { return configVersion; }
        }

        // Common
        static private string siteSpecificDelimiter;
        static public string SiteSpecificDelimiter
        {
            get { return siteSpecificDelimiter; }
        }

        private static bool highSpeedApiEnabled;
        public static bool HighSpeedApiEnabled
        {
            get { return highSpeedApiEnabled; }
        }

        private static bool isIPv6Enabled ;

        public static bool IsIPv6Enabled
        {
            get { return isIPv6Enabled; }
        }

        private static bool isIPv4Enabled;

        public static bool IsIPv4Enabled
        {
            get { return isIPv4Enabled; }
        }
               
        private static string dbDataPath;        
		/// <summary>file path for the data files</summary>
        public static string DBDataPath
        {
            get { return dbDataPath; }
        }

        // GeoLocationAPI
        static private int maximumCallTimeoutInterval;
		static public int MaximumCallTimeoutInterval
        {
            get { return maximumCallTimeoutInterval; }
        }

        static private int socketConnectionStatusTimerInterval;
		static public int SocketConnectionStatusTimerInterval
		{
			get { return socketConnectionStatusTimerInterval; }
		}

        private static string postalCodeSeparatorCharacter;
        public static string PostalCodeSeparatorCharacter
        {
            get {return postalCodeSeparatorCharacter;}
        }

        private static string postalCodeRangeSeperatorCharacter;
        public static string PostalCodeRangeSeperatorCharacter
        {
            get {return postalCodeRangeSeperatorCharacter;}
        }

        private static int maxDBErrorLogPerMinute;
        public static int MaxDBErrorLogPerMinute
        {
            get {return maxDBErrorLogPerMinute;}
        }

        private static int maxSocketErrorLogPerMinute;
        public static int MaxSocketErrorLogPerMinute
        {
            get {return maxSocketErrorLogPerMinute;}
        }

        private static int maxStatsGetPerMinute;
        public static int MaxStatsGetPerMinute
        {
            get {return maxStatsGetPerMinute;}
        }

        private static int maxStatsSetPerMinute;
        public static int MaxStatsSetPerMinute
        {
            get {return maxStatsSetPerMinute;}
        }

        private static string mapPointIdQueryStringName = "MPID";        		
		public static string MapPointIdQueryStringName
        {
            get {return mapPointIdQueryStringName;}
        }
        private static string extendedPropertyConfigFile;		
		public static string ExtendedPropertyConfigFile
        {
            get {return extendedPropertyConfigFile;}
        }

        private static string extendedPropertySchemaFile = "ExtendedProperties.xsd";
		public static string ExtendedPropertySchemaFile
        {
            get {return extendedPropertySchemaFile;}
        }

        private static string geoClosestLocationSchemaFile = "GeoClosestLocations.xsd";
        public static string GeoClosestLocationSchemaFile
        {
            get { return geoClosestLocationSchemaFile; }
        }


        private static string geoClosestLocationConfigFile;
        public static string GeoClosestLocationConfigFile
        {
            get { return geoClosestLocationConfigFile; }
        }

        // Database
        static private string dbConnection;
        static public string DBConnection
        {
            get { return dbConnection; }
        }

        // GeoSocket        
        private static int resultCacheLength;
        /// <summary>ResultCache Capacity (how many items can be in cache)</summary>
        public static int ResultCacheLength
        {
            get { return resultCacheLength; }
        }

        private static int tcpListenerMaxThreads;
        /// <summary>TCP Listener max threads. Should >= 25</summary>
        public static int TcpListenerMaxThreads
        {
            get { return tcpListenerMaxThreads; }
        }

        private static int tcpListenerMaxReceiveTimeout;
        /// <summary>When a connection has not made requests for N seconds, stop it by timeout</summary>
        public static int TcpListenerMaxReceiveTimeout
        {
            get { return tcpListenerMaxReceiveTimeout; }
        }

        private static int tcpListenerManagingInterval;
        /// <summary>Every N seconds ManagingThreads will wake up to maintain the threads</summary>
        public static int TcpListenerManagingInterval
        {
            get { return tcpListenerManagingInterval; }
        }

        private static int tcpListenerTimeoutForSync;
        /// <summary>When data files are republished, wait for this amount of time so all files are updated, then load</summary>
        public static int TcpListenerTimeoutForSync
        {
            get { return tcpListenerTimeoutForSync; }
        }

        private static float safePaddingRatioToThreadPoolCapacity;
        /// <summary>when there are more connections than TCPListenerMaxThreads, we force threads to stop and leave this ratio (SafePaddingRatioToThreadPoolCapacity*TCPListenerMaxThreads) of pools empty for new connections to work</summary>
		public static float SafePaddingRatioToThreadPoolCapacity
        {
            get {return safePaddingRatioToThreadPoolCapacity;}
        }

        private static bool performanceMonitoringEnabled = true;
        /// <summary>Enable perfmon or not</summary>
        public static bool PerformanceMonitoringEnabled
        {
            get { return performanceMonitoringEnabled; }
            set { performanceMonitoringEnabled = value; } // if anything wrong we allow external to reset it.
        }

        private static bool cacheEnabled;
		/// <summary>Enable cache or not</summary>
        public static bool CacheEnabled
        {
            get { return cacheEnabled; }
        }

        static int tcpListenerPortNumber = 1234;
        /// <summary>TCP port number from configure file</summary>
        public static int TcpListenerPortNumber
        {
            get { return tcpListenerPortNumber; }
        }

        static string geoResultDelimiter = ";";
        /// <summary>The delimitor to seperate the result string. Suggest use a char instead of a string</summary>
        public static string GeoResultDelimiter
        {
            get { return geoResultDelimiter; }
        }

        static int defaultLocale = 9;
        /// <summary>if localized string is not found, then fall back to this locale</summary>
        public static int DefaultLocale
        {
            get { return defaultLocale; }
        }
                
        static private int threshHoldToRetryConnection;
        static public int ThreshHoldToRetryConnection
        {
            get { return threshHoldToRetryConnection; }
        }

        /// <summary>Client side timeout value. So no need to serve request after that duration.</summary>
        static private int socketClientSideTimeout;
        static public int SocketClientSideTimeout
        {
            get { return socketClientSideTimeout; }
        }

        static private string socketServerName;
        static public string SocketServerName
        {
            get { return socketServerName; }
        }

        static private string socketNotFound;
        /// <summary>the string for socket server to return when nothing is found but there is no error</summary>
        static public string SocketNotFound
        {
            get { return socketNotFound; }
        }

        static private string socketException;
        /// <summary>the string for socket server to return when there is an internal exception in socket server</summary>
        static public string SocketException
        {
            get { return socketException; }
        }

        static int safeOpenFileTimeout = 30;
        /// <summary>When read a data file, we can allow SafeOpenFileTimeOut times retry on IOException. Each retry takes 1 second.</summary>
        public static int SafeOpenFileTimeout
        {
            get { return safeOpenFileTimeout; }
        }

        static int receiveBufferSizeInBytes = 256;
        /// <summary>this is the buffer size for socket communication -- socket client to receive information</summary>
        public static int ReceiveBufferSizeInBytes
        {
            get { return receiveBufferSizeInBytes; }
        }

        static int sendBufferSizeInBytes = 256;
        /// <summary>this is the buffer size for socket communication -- socket client to send query (usually just to store IP and locale)</summary>
        public static int SendBufferSizeInBytes
        {
            get { return sendBufferSizeInBytes; }
        }

        static bool cacheSocketConnection = true;
        /// <summary>tells whether cache socket communication</summary>
        public static bool CacheSocketConnection
        {
            get { return cacheSocketConnection; }
        }

        // StatsNet
        static private bool enableStatsNetLogging;
        static public bool EnableStatsNetLogging
        {
            get { return enableStatsNetLogging; }
        }

        static private string statsSubscription;
        static public string StatsSubscription
        {
            get { return statsSubscription; }
        }

        static private int proxyTimeout;
        static public int ProxyTimeout
        {
            get { return proxyTimeout; }
        }
                
        //GeoProfileProvider 
        static private string attributeId;
        static public string AttributeId
        {
            get { return attributeId; }
        }

        static private string attributeGroupId;
        static public string AttributeGroupId
        {
            get { return attributeGroupId; }
        }

        static private bool skipAnonDBUpdate = true;

        public static bool SkipAnonDBUpdate
        {
            get { return skipAnonDBUpdate; }
          
        }
 

        static private string domain;
        static public string Domain
        {
            get { return domain; }
        }

        static string netScalarIPHeader;
        static public string NetScalarIPHeader
        {
            get { return netScalarIPHeader; }
        }



        static string statsNetProxyUrl1 ;
        public static string StatsNetProxyUrl1
        {
          get { return statsNetProxyUrl1; }
        }


        static string statsNetProxyUrl2;
        public static string StatsNetProxyUrl2
        {
            get { return statsNetProxyUrl2; }
        }



        public static event EventHandler ExtPropFileNameChangeHandler ;

        static char erQueryEndChar = '$';
        public static char ERQueryEndChar
        {
            get { return erQueryEndChar; }
        }

        #endregion

        // Static constructor, loading configuration
		static MSCOMGeoConfig()
		{			
            configPath = ConfigurationManager.AppSettings["GeoConfigFilePathV2"];

			if (configPath != "NOCONFIGFILE"
                && configPath != null)
			{
                if (string.IsNullOrEmpty(configPath))
                    configPath = @"c:\appconfig\geosegmentation\v2\Geo.MSCOM.config";
				LoadUnChangableFromAppsConfig();
				LoadFromAppsConfig(null, null);

				watcher = new FileSystemWatcher();
				watcher.Path = Path.GetDirectoryName(configPath);
				watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;                    
                string filename = Path.GetFileName(configPath);
                // Note: for filter, I cannot directly use filename because it is geo.mscom.config, seems filesystemwatcher cannot monitor this kind of filename.
                // so I have to use one of the following *.*, geo.*, geo.mscom.*. I choose not to use *.* because extendedproperty.config might be in same folder.
                int lastDot = filename.LastIndexOf('.');
                watcher.Filter = filename.Substring(0, lastDot + 1) + "*";
				watcher.Changed += new FileSystemEventHandler(MSCOMGeoConfig.LoadFromAppsConfig);
                watcher.Created += new FileSystemEventHandler(MSCOMGeoConfig.LoadFromAppsConfig);
				watcher.EnableRaisingEvents = true;
			}
			else if(configPath == "NOCONFIGFILE")
			{
				// These are needed in publisher. Compare with LoadFromAppsConfig	
				dbDataPath = ConfigurationManager.AppSettings["DataFilePath"];
                if (string.IsNullOrEmpty(dbDataPath))
                    dbDataPath = @"c:\appconfig\geosegmentation\v2\data\";
				geoResultDelimiter = Helper.GetConfigureString("GeoResultDelimitor", ";");
				defaultLocale = Int32.Parse(Helper.GetConfigureString("DefaultLocale", "9"), CultureInfo.InvariantCulture);
                isIPv6Enabled = MSCOMGeoConfig.ReadIsIPv6Enabled();
                isIPv4Enabled = MSCOMGeoConfig.ReadIsIPv4Enabled();
			}
            else // configPath == null
            {
                // These are needed in publisher. Compare with LoadFromAppsConfig	
                dbDataPath = string.Empty;
                geoResultDelimiter = ";";
                defaultLocale = 9;
                isIPv6Enabled = true;
                isIPv4Enabled = true;
            }
			
		}

        //=====================================================================
        //  Method: IsIPv4Enabled
        //
        /// <summary>
        /// Reads bool value, from the app.config file for IsIPv4Enabled
        /// </summary>  
        //=====================================================================


        private static bool ReadIsIPv4Enabled()
        {
            bool isIPv4Enabled = true;
            //
            //  Get the GeoLocationPosition
            //

            string isIPv4EnabledValue = ConfigurationManager.AppSettings[MSCOMGeoConfig.IsIPv4EnabledKey];

            if (string.IsNullOrEmpty(isIPv4EnabledValue) == false)
            {
                bool succeeded = bool.TryParse(
                    isIPv4EnabledValue,
                    out isIPv4Enabled
                    );

                if (succeeded == false)
                {
                    isIPv4Enabled = true;
                }
            }

            return isIPv4Enabled;
        }

        //=====================================================================
        //  Method: IsIPv6Enabled
        //
        /// <summary>
        /// Reads bool value, from the app.config file for IsIPv6Enabled
        /// </summary>  
        //=====================================================================


        private static bool ReadIsIPv6Enabled()
        {
            bool isIPv6Enabled = true;
            //
            //  Get the GeoLocationPosition
            //

            string isIPv6EnabledValue = ConfigurationManager.AppSettings[MSCOMGeoConfig.IsIPv6EnabledKey];

            if (string.IsNullOrEmpty(isIPv6EnabledValue) == false)
            {
                bool succeeded = bool.TryParse(
                    isIPv6EnabledValue,
                    out isIPv6Enabled
                    );

                if (succeeded == false)
                {
                    isIPv6Enabled = true;
                }
            }

            return isIPv6Enabled;
        }

        static string getXmlConfigureString(XmlElement root, string category, string configureName)
        {
            return getXmlConfigureString(root, category, configureName, true);
        }

        static string getXmlConfigureString(XmlElement root, string category, string configureName, bool allowOverride)
        {
            if (allowOverride)
            {
                string result = ConfigurationManager.AppSettings[configureName];
                if (!string.IsNullOrEmpty(result))
                    return result;
            }
            string xquery;
            XmlNode resultNode = null;
            if (SiteSpecificDelimiter != null)
            {
                xquery = "project[@name='" + projectName + "']/category[@name='" + category + "']/setting[@name='" + configureName + SiteSpecificDelimiter + "']/@value";
                resultNode = root.SelectSingleNode(xquery);
            }
            if (resultNode == null)
            {
                xquery = "project[@name='" + projectName + "']/category[@name='" + category + "']/setting[@name='" + configureName + "']/@value";
                resultNode = root.SelectSingleNode(xquery);
            }
            if (resultNode == null)
            {
               //EventLogWriter.WriteEntry(EventType.Configuration, "Cannot get configure value for " + configureName);
                return null;
            }
            else
            {
                return resultNode.InnerText;
            }
        }

		/// <summary>
		/// Loads the un-changable config settings from apps config.
		/// </summary>
		/// <remarks>
		/// All config settings loaded here cannot be changed during runtime to take effect on the fly. They are monitored by the file watcher.
		/// </remarks>
		private static void LoadUnChangableFromAppsConfig()
		{
            lock (syncRoot)
			{
				XmlDocument appDoc = SafeOpenXmlDocument(configPath);
				if (appDoc == null)
				{
                    //EventLogWriter.WriteEntry(EventType.Configuration, "MSCOMGeoConfig:LoadUnChangableFromAppsConfig at " + configPath + "  failed");
					return;
				}
				XmlElement appRootElement = appDoc.DocumentElement;
					
                    
                string tempValue;
				// If this flag is also set by user in app.config, it should not be overwritten. There will be no senario to deal with runing multiple Geo applications with different high speed settings on the same box sharing the same Geo config file.
                tempValue = getXmlConfigureString(appRootElement, "Common", "HighSpeedAPIEnabled", false);

                // if conversion fails, set it to default
                if (bool.TryParse(tempValue, out highSpeedApiEnabled) == false)
                {
                    highSpeedApiEnabled = false;
                }

                tempValue = getXmlConfigureString(appRootElement, "GeoSocket", "TCPListenerPortNumber", false);
                tcpListenerPortNumber = Helper.GetSafeConfigureInteger("TCPListenerPortNumber", tempValue, 1, Int32.MaxValue, 1234);

                dbDataPath = getXmlConfigureString(appRootElement, "Common", "DataFilePath");
                    
                if (string.IsNullOrEmpty(dbDataPath))
                    dbDataPath = @"c:\appconfig\geosegmentation\v2\data\";

                extendedPropertySchemaFile = getXmlConfigureString(appRootElement, "GeoLocationAPI", "ExtendedPropertySchemaFileName", true);
                geoClosestLocationSchemaFile = getXmlConfigureString(appRootElement, "GeoLocationAPI", "GeoClosestLocationSchemaFileName", true);

                tempValue = getXmlConfigureString(appRootElement, "Common", IsIPv4EnabledKey, false);
                    
                // if conversion fails, set it to default
                if (bool.TryParse(tempValue, out MSCOMGeoConfig.isIPv6Enabled) == false)
                {
                    MSCOMGeoConfig.isIPv6Enabled = false;
                }

                tempValue = getXmlConfigureString(appRootElement, "Common", IsIPv6EnabledKey, false);

                // if conversion fails, set it to default
                if (bool.TryParse(tempValue, out MSCOMGeoConfig.isIPv4Enabled) == false)
                {
                    MSCOMGeoConfig.isIPv4Enabled = false;
                }
				
			}
		}

		// LoadFromAppConfig is the method that load a file and get all its configuration node to
		// member data in this class.
		private static void LoadFromAppsConfig(object source, FileSystemEventArgs e)
		{
			lock (syncRoot)
			{
				if (((TimeSpan)DateTime.Now.Subtract(lastLoad)).TotalSeconds <= 5)
					return; // duplicate events
                fileLastModified = File.GetLastWriteTime(configPath);
				XmlDocument appDoc;
				try
				{
					appDoc = SafeOpenXmlDocument(configPath);
					if (appDoc == null)
					{
						//EventLogWriter.WriteEntry(EventType.Configuration, "MSCOMGeoConfig:LoadFromAppsConfig at " + configPath + "  failed");
						return;
					}
					XmlElement appRootElement = appDoc.DocumentElement;


                    #region Load all configures
                    string temp;                   
                    
                    // version
                    XmlNode configNode = appRootElement.SelectSingleNode("/config");
                    if (configNode != null)
                    {
                        // Extract attribute                        
                        configVersion = configNode.Attributes["version"].Value;                      
                    }
                    
                    // Common
                    string section = "Common";
                    siteSpecificDelimiter = getXmlConfigureString(appRootElement, section, "SiteSpecificDelimiter");
                                                            
                    // GeoLocationAPI
                    section = "GeoLocationAPI";

                    temp = getXmlConfigureString(appRootElement, section, "MaximumCallTimeoutInterval");
                    maximumCallTimeoutInterval = Helper.GetSafeConfigureInteger("MaximumCallTimeoutInterval", temp, 0, Int32.MaxValue, 3);

                    temp = getXmlConfigureString(appRootElement, section, "SocketConnectionStatusTimerInterval");
                    socketConnectionStatusTimerInterval = Helper.GetSafeConfigureInteger("SocketConnectionStatusTimerInterval", temp, 0, Int32.MaxValue, 4000);

                    postalCodeSeparatorCharacter = getXmlConfigureString(appRootElement, section, "PostalCodeSeperatorCharacter");   
                
                    postalCodeRangeSeperatorCharacter = getXmlConfigureString(appRootElement, section, "PostalCodeRangeSeperatorCharacter");   

                    temp = getXmlConfigureString(appRootElement, section, "DBErrorLogPerMinute");
                    maxDBErrorLogPerMinute = Helper.GetSafeConfigureInteger("DBErrorLogPerMinute", temp, 0, Int32.MaxValue, 1);

                    temp = getXmlConfigureString(appRootElement, section, "SocketErrorLogPerMinute");
                    maxSocketErrorLogPerMinute = Helper.GetSafeConfigureInteger("SocketErrorLogPerMinute", temp, Int32.MinValue, Int32.MaxValue, 1);

                    temp = getXmlConfigureString(appRootElement, section, "StatsGetPerMinute");
                    maxStatsGetPerMinute = Helper.GetSafeConfigureInteger("StatsGetPerMinute", temp, Int32.MinValue, Int32.MaxValue, 100);

                    temp = getXmlConfigureString(appRootElement, section, "StatsSetPerMinute");
                    maxStatsSetPerMinute = Helper.GetSafeConfigureInteger("StatsSetPerMinute", temp, Int32.MinValue, Int32.MaxValue, 100);

                    mapPointIdQueryStringName = getXmlConfigureString(appRootElement, section, "MapPointIDQueryStringName");

                    temp = getXmlConfigureString(appRootElement, section, "ExtendedPropertyConfigFile", true);

                    if ( !string.IsNullOrEmpty(temp)) {
                      if ( string.IsNullOrEmpty(extendedPropertyConfigFile) 
                          || string.Compare(temp, extendedPropertyConfigFile, StringComparison.OrdinalIgnoreCase) != 0)
                      {
                        //different file than before, call Event Handler
                        extendedPropertyConfigFile = temp;

                     //   ExtPropFileNameChangeHandler.Invoke(new Object() , EventArgs.Empty);
                          if ( ExtPropFileNameChangeHandler != null)
                            ExtPropFileNameChangeHandler( new Object() , EventArgs.Empty);
                      }
                    }

                    geoClosestLocationConfigFile = getXmlConfigureString(appRootElement, "GeoLocationAPI", "GeoClosestLocationConfigFile", true);

                    // EventLog
                    section = "EventLog";
                   //EventLogWriter.EventLogDetail = ("1" == getXmlConfigureString(appRootElement, section, "EventDetail"));
                   //EventLogWriter.EventLogCategory = getXmlConfigureString(appRootElement, section, "EventLog");
                   //EventLogWriter.EventLogName = getXmlConfigureString(appRootElement, section, "EventSource");
                    temp = getXmlConfigureString(appRootElement, section, "EventLogLevel");
                   //EventLogWriter.EventLogLevel = Helper.GetSafeConfigureInteger("EventLogLevel", temp, 0, 2, 0);

                    // Database
                    section = "Database";
                    dbConnection = getXmlConfigureString(appRootElement, section, "GeoSystemDatabase");   
                                        
                    // GeoSocket        
                    section = "GeoSocket";
                    
                    temp = getXmlConfigureString(appRootElement, section, "ResultCacheLength");
                    resultCacheLength = Helper.GetSafeConfigureInteger("ResultCacheLength", temp, 0, Int32.MaxValue, 1000000);

                    temp = getXmlConfigureString(appRootElement, section, "TCPListenerMaxThreads");
                    tcpListenerMaxThreads = Helper.GetSafeConfigureInteger("TCPListenerMaxThreads", temp, 0, Int32.MaxValue, 1000);

                    temp = getXmlConfigureString(appRootElement, section, "TCPListenerMaxRecvTimeout");
                    tcpListenerMaxReceiveTimeout = Helper.GetSafeConfigureInteger("TCPListenerMaxRecvTimeout", temp, 0, Int32.MaxValue, 5000);

                    temp = getXmlConfigureString(appRootElement, section, "TCPListenerManagingInterval");
                    tcpListenerManagingInterval = Helper.GetSafeConfigureInteger("TCPListenerManagingInterval", temp, 0, Int32.MaxValue, 4000);

                    temp = getXmlConfigureString(appRootElement, section, "TCPListenerTimeoutForSync");
                    tcpListenerTimeoutForSync = Helper.GetSafeConfigureInteger("TCPListenerTimeoutForSync", temp, 0, Int32.MaxValue, 15000);

                    temp = getXmlConfigureString(appRootElement, section, "SafePaddingRatioToThreadPoolCapacity");
                    if (float.TryParse(temp, out safePaddingRatioToThreadPoolCapacity) == false)
                    {
                        safePaddingRatioToThreadPoolCapacity = 0;
                    }

                    temp = getXmlConfigureString(appRootElement, section, "PerfMonEnabled");
                    if (bool.TryParse(temp, out performanceMonitoringEnabled) == false)
                    {
                        performanceMonitoringEnabled = false;
                    }

                    temp = getXmlConfigureString(appRootElement, section, "CacheEnabled");
                    if (bool.TryParse(temp, out cacheEnabled) == false)
                    {
                        cacheEnabled = false;
                    }
                    
                    geoResultDelimiter = getXmlConfigureString(appRootElement, section, "GeoResultDelimitor");

                    temp = getXmlConfigureString(appRootElement, section, "DefaultLocale");
                    defaultLocale = Helper.GetSafeConfigureInteger("DefaultLocale", temp, 0, Int32.MaxValue, 9);

                    temp = getXmlConfigureString(appRootElement, section, "ThreshholdToRetryConnection");
                    threshHoldToRetryConnection = Helper.GetSafeConfigureInteger("ThreshholdToRetryConnection", temp, 0, Int32.MaxValue, 3);

                    temp = getXmlConfigureString(appRootElement, section, "SocketClientSideTimeout");
                    socketClientSideTimeout = Helper.GetSafeConfigureInteger("SocketClientSideTimeout", temp, 0, Int32.MaxValue, 15);

                    socketServerName = getXmlConfigureString(appRootElement, section, "SocketServerName", false);

                    socketNotFound = getXmlConfigureString(appRootElement, section, "Socket_NotFound_Return");

                    socketException = getXmlConfigureString(appRootElement, section, "Socket_Exception_Return");

                    temp = getXmlConfigureString(appRootElement, section, "SafeOpenFileTimeOut");
                    safeOpenFileTimeout = Helper.GetSafeConfigureInteger("SafeOpenFileTimeOut", temp, 0, Int32.MaxValue, 30);        

                    temp = getXmlConfigureString(appRootElement, section, "ReceiveBufferSizeInBytes");
                    receiveBufferSizeInBytes = Helper.GetSafeConfigureInteger("ReceiveBufferSizeInBytes", temp, 0, Int32.MaxValue, 256);        

                    temp = getXmlConfigureString(appRootElement, section, "SendBufferSizeInBytes");
                    sendBufferSizeInBytes = Helper.GetSafeConfigureInteger("SendBufferSizeInBytes", temp, 0, Int32.MaxValue, 256);        

                    temp = getXmlConfigureString(appRootElement, section, "SocketConnectionIsCachable");
                    if (bool.TryParse(temp, out cacheSocketConnection) == false)
                    {
                        cacheSocketConnection = false;
                    }

                    //StatsNet
                    section = "StatsNet";

                    temp = getXmlConfigureString(appRootElement, section, "EnableStatsNetLogging");
                    if (bool.TryParse(temp, out enableStatsNetLogging) == false)
                    {
                        enableStatsNetLogging = false;
                    }

                    statsSubscription = getXmlConfigureString(appRootElement, section, "StatsSubscription");

                    statsNetProxyUrl1 = getXmlConfigureString(appRootElement, section, "StatsNetProxyURL1");
                    statsNetProxyUrl2 = getXmlConfigureString(appRootElement, section, "StatsNetProxyURL2");

                    temp = getXmlConfigureString(appRootElement, section, "ProxyTimeout");
                    proxyTimeout = Helper.GetSafeConfigureInteger("ProxyTimeout", temp, 0, Int32.MaxValue, 5000);        

                    //GeoProfileProvider 
                    section = "GeoProfileProvider";

                    attributeId = getXmlConfigureString(appRootElement, section, "AttributeId");
        
                    attributeGroupId = getXmlConfigureString(appRootElement, section, "AttributeGroupId");

                    string skipAnon = getXmlConfigureString(appRootElement, section, "SkipAnonDBUpdate");
                    if (string.IsNullOrEmpty(skipAnon) || !bool.TryParse(skipAnon , out skipAnonDBUpdate))
                        skipAnonDBUpdate = true;
        
                    domain = getXmlConfigureString(appRootElement, section, "Domain");
        
                    netScalarIPHeader = getXmlConfigureString(appRootElement, section, "NetscalerIPHeader");
                    #endregion                    
                    
                    // GeoLocation API Settings	
					setMaxThreadPoolThreads();			
				}
				catch (Exception)
				{
					//EventLogWriter.WriteException(EventType.Configuration, "MSCOMGeoConfig:LoadFromAppsConfig at " + configPath + "  failed", ex);
				}
				finally
				{
					lastLoad = DateTime.Now;
				}
			}
		}

		/// <summary>
		/// Gets the configuration from geo configuration file.
		/// </summary>
		/// <returns>The XML DOM object of the configuration file.</returns>
		/// <remarks>To improve the information provided for internal usage (for example, on Smoke Test Pages).</remarks>
		public static XmlDocument GetConfigDoc()
		{
			return SafeOpenXmlDocument(configPath);
		}

		/// <summary>
		/// Open file safely for read access 
		/// When we open an xmlDocument file for read and something is writing this file, we should wait until that is done.
		/// </summary>
		/// <param name="fileName">filename for read</param>		
		/// <returns>XmlDocument for read</returns>
		public static XmlDocument SafeOpenXmlDocument(string fileName)
		{
            if (!File.Exists(fileName))
            {
               //EventLogWriter.WriteEntry(EventType.Configuration, "MSCOMGeoConfig:SafeOpenXmlDocument: Unable to find: " + fileName);
                return null;
            }
			XmlDocument document = null;
			int numOfTries = 0;

			while (true)
			{
			    try
			    {
			        document = new XmlDocument();
			        document.Load(fileName);
			        break;
			    }
			    catch (IOException)
			    {
			        if (numOfTries < SafeOpenFileTimeout)
			        {
			            numOfTries++;
			            Thread.Sleep(1000);
			        }
			        else
			        {
			            throw;
			        }
			    }
			}
			return document;
		}

		private static void setMaxThreadPoolThreads()
		{
			int th1, th2;
			ThreadPool.GetMaxThreads(out th1, out th2);
			int numNewThreads = Math.Max(Environment.ProcessorCount * 25, (int)(MSCOMGeoConfig.TcpListenerMaxThreads * 1.2)); // Default is 25 threads per CPU. But we might need more
			if (th1 < numNewThreads)
				ThreadPool.SetMaxThreads(numNewThreads, th2);
		}
	}
}
