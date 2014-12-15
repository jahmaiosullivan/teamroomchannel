using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.IT.Geo.Legacy.GeoLocationApi;
using Microsoft.MSCOM.Geo.GeoLocationAPI;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoCommon
{
	/// <summary>
	/// This is a simple cache instance sit inside TCPListener. It accepts the IP address and return the GeoID
	/// The caching method is very simple, just cache the first "MSCOMGeoConfig.ResultCacheLength" unique IPAddresses. 
	/// Then never change the cache until a refresh. This way we can reduce the caching cost to minimal while still have some caching benefit
	/// </summary>
	public sealed class ResultCache
	{
        static Dictionary<string, int> geoIndexCache;

        private static Dictionary<string, int>  GeoIndexCache
        {
            get
            {
                if (geoIndexCache == null)
                {
                    geoIndexCache = new Dictionary<string, int>(MSCOMGeoConfig.ResultCacheLength + 1024); // 1024 is a padding
                }

                return ResultCache.geoIndexCache;
            }
        }

		static bool cacheAvailable = MSCOMGeoConfig.CacheEnabled;
		static Timer timer = null;		

#if DEBUG
		static bool cacheFullEvent;
#endif

		private ResultCache() { } // prevent people to create instance.

		static ResultCache()
		{
			timer = new Timer(new TimerCallback(timerProc));
			int resultCacheRefreshIntervalSeconds = Helper.GetConfigureInteger("ResultCacheRefreshIntervalSeconds", 3600, Int32.MaxValue, 86400);
			timer.Change(resultCacheRefreshIntervalSeconds * 1000, resultCacheRefreshIntervalSeconds * 1000); 
		}
		/// <summary>
		/// after Calling this method, you must call Cleanup to make cache back online
		/// </summary>
		static public void DisableCache()
		{
			cacheAvailable = false;
		}
		/// <summary>Cache clean up</summary>
		static public void Cleanup()
		{
			if (!cacheAvailable)
				return;
           //EventLogWriter.WriteEntry(EventType.GeoCommon, "ResultCache:Begin to refresh result cache", System.Diagnostics.EventLogEntryType.Information);
			lock (geoIndexCache)
			{
				cacheAvailable = false;
				Thread.Sleep(30000); // wait for all threads exit the public methods
                ResultCache.GeoIndexCache.Clear();
				cacheAvailable = true;
			}
#if DEBUG
			cacheFullEvent = false;
#endif
		}

		static private void timerProc(object state)
		{
			if (!cacheAvailable)
				return;			
			Cleanup();
		}


		/// <summary>Insert (IP, GeoIndex) into memory</summary>
		static public void InsertIntoCache(string ip, int geoIndex)
		{
            if (cacheAvailable 
                && (ResultCache.GeoIndexCache.Count < MSCOMGeoConfig.ResultCacheLength))
			{
				lock (geoIndexCache)
				{
					ResultCache.GeoIndexCache[ip] = geoIndex;
				}
			}
#if DEBUG
            if (ResultCache.GeoIndexCache.Count == MSCOMGeoConfig.ResultCacheLength
                && (!cacheFullEvent))
			{
				cacheFullEvent = true;
               //EventLogWriter.WriteEntry(EventType.GeoCommon, "ResultCache:Result cache has been built up.", System.Diagnostics.EventLogEntryType.Information);
			}
#endif
		}
	}

	/// <summary>
	/// This is the class to keep all data inside. When data file changes, we will create another instance of this class, swap with old instance.
	/// </summary>
	public class GeoInfoLookup
	{
		static char[] splitorDot = new char[] { '.' };
        
        SubnetBeginAndGeoIdList geoInfoData = new SubnetBeginAndGeoIdList(); // 0-32

        // SubnetBeginGeoIdMapping geoInfoData = new SubnetBeginGeoIdMapping(true); // 0-32

        SubnetBeginGeoIdMapping geoInfoDataIPv6 = new SubnetBeginGeoIdMapping(false); // 0-128

        SubnetRangeLookup reservedLookupIPv4 = new SubnetRangeLookup(true); // 0-32
        SubnetRangeLookup reservedLookupIPv6 = new SubnetRangeLookup(false); // 0-32

		GeoCountryRegion geoCountryRegion = new GeoCountryRegion();
		GeoCity geoCity = new GeoCity();
		GeoStateProvince geoStateProvince = new GeoStateProvince();
		GeoTimeZone geoTimeZone = new GeoTimeZone();
		GeoIndex geoIndex = new GeoIndex();
		GeoPosition geoPosition = new GeoPosition();
		GeoDataVersion geoDataVersion = new GeoDataVersion();
		GeoCountryRegionCodeLookup geoCRCodeLookup = new GeoCountryRegionCodeLookup();
		GeoStateProvinceCodeLookup geoSPCodeLookup = new GeoStateProvinceCodeLookup();
		GeoLocaleLookup geoLocaleLookup = new GeoLocaleLookup();
        GeoLanguageLookup geoLanguageLookup = new GeoLanguageLookup();
        GeoEntityRelation geoEntityRelation = new GeoEntityRelation();
        GeoPostalCodeLookup geoPostalCodeLookup = new GeoPostalCodeLookup();

        static string fileNameWithoutPath = "MSCOMGeoInfoServerData.dat";
        public static string FileNameWithoutPath
        {
            get { return fileNameWithoutPath; }
        }

        static string fileName;

		public static string FileName
        {
            get 
            {
                if (fileName == null)
                {
                    GeoInfoLookup.fileName = Path.Combine(
                        MSCOMGeoConfig.DBDataPath, 
                        fileNameWithoutPath
                        );
                }

                return fileName; 
            }
		}
		static string backupFileName;
		public static string BackupFileName
		{
			get
            {
                if (backupFileName == null)
                {
                    backupFileName = Path.Combine(
                        MSCOMGeoConfig.DBDataPath,
                        "MSCOMGeoInfoServerData.bak"
                        );
                }

                return backupFileName; 
            }
		}
						
		/// <summary>
		/// Given IP in a string, convert it into integer
		/// </summary>
		/// <param name="ipInString"></param>
		/// <returns></returns>
		static public int ConvertIPToBytes(string ipString)
		{			
			if (String.IsNullOrEmpty(ipString))
				return 0;
			string[] elements = ipString.Split(splitorDot);
			if (elements.Length == 4)
			{
				/*
				ip = Convert.ToInt32(elements[0], 10) << 24;
				ip |= Convert.ToInt32(elements[1], 10) << 16;
				ip |= Convert.ToInt32(elements[2], 10) << 8;
				ip |= Convert.ToInt32(elements[3], 10);
				 */
				return Convert.ToInt32(elements[0], 10) << 24 
					| Convert.ToInt32(elements[1], 10) << 16
					| Convert.ToInt32(elements[2], 10) << 8
					| Convert.ToInt32(elements[3], 10);
			}
			else
				return 0;
		}

	    public GeoInfoLookup() : this(null)
	    {
	        
	    }
		/// <summary>
		/// Load data from either DB or file, and save to file if asked to do
		/// </summary>
		public GeoInfoLookup(Stream dataStream)
		{			
			if (dataStream != null)
            {
                using (var r = new BinaryReader(dataStream))
                {
#if DEBUG
                    int tickStart = Environment.TickCount;
#endif
                    this.Read(r);
#if DEBUG
                    Console.WriteLine("Takes " + (Environment.TickCount - tickStart) + " milliseconds to load everything from file");
#endif
                    //r.Close();
                }
            }
		}

		public static void BackupFile()
		{
			try
			{
				if (File.Exists(backupFileName))
					File.SetAttributes(backupFileName, FileAttributes.Normal);
				File.Copy(fileName, backupFileName, true);
			}
			catch (Exception)
			{
				//EventLogWriter.WriteException(EventType.GeoCommon, "GeoInfoLookup:Failed in backup the last known good file", e);
				// eat the error as this does not prevent the server from running
			}
		}

		/// <summary>
		/// Lookup GeoID by input IP string
		/// </summary>		
		protected int Lookup(string ipString)
		{
			int geoId = -1;

            IPAddress ipAddress = null;

            bool isValidAddress = IPAddress.TryParse(
                ipString,
                out ipAddress
                );

            if (isValidAddress != false)
            {
                //if (MSCOMGeoConfig.CacheEnabled)
                //{
                //    geoId = ResultCache.GetGeoIndexByIP(ipStr);
                //    if (geoId >= 0)
                //    {
                //        return geoId;
                //    }
                //}

                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    int ip = ConvertIPToBytes(ipString);

                    geoId = this.geoInfoData.Lookup(ip);

                    // geoId = this.geoInfoData.Lookup(ipAddress);
                }
                else if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    geoId = this.geoInfoDataIPv6.Lookup(ipAddress);
                }


                //if (MSCOMGeoConfig.CacheEnabled && geoId > -1)
                //{
                //    ResultCache.InsertIntoCache(ipStr, geoId);
                //}

            }
            return geoId;
		}

        /// <summary>
        /// Lookup GeoID by input IP string
        /// </summary>		
        public bool IsReserved(string ipStr)
        {
            bool isReserved = false;

            IPAddress ipAddress;

            bool isValidAddress = IPAddress.TryParse(
                ipStr,
                out ipAddress
                );

            if (isValidAddress != false)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    isReserved = this.reservedLookupIPv4.IsReserved(ipAddress);

                    // geoId = this.geoInfoData.Lookup(ipAddress);
                }
                else if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    isReserved = this.reservedLookupIPv6.IsReserved(ipAddress);
                }
            }

            return isReserved;
        }

		public void GetGeoDataVersion(StringBuilder geoResult)
		{
			if (geoResult != null)
			{
				geoResult.Append(geoDataVersion.DataVersion).Append(MSCOMGeoConfig.GeoResultDelimiter);
			}
		}

		/// <summary>
		/// Gets the geo info by mappoint ID.
		/// </summary>
		/// <param name="ID">The ID.</param>
		/// <param name="localeID">The locale ID.</param>
		/// <param name="zipCodeMode">The zip code mode.</param>
		/// <returns>
		/// The GeoInfoCommon object containing geo information. Can be null.
		/// </returns>
		/// <remarks>New API for high speed in process access without serialization to string.</remarks>
		public GeoInfoCommon GetGeoInfoByMappointId(int id, string localeId, int zipCodeMode)
		{
			GeoInfoCommon geoInfo;
			try
			{
				GeoData geoData = null;
				if (!geoIndex.GeoDataByID.TryGetValue(id, out geoData))
				{
					geoInfo = null;
				}
				else
				{
					geoInfo = new GeoInfoCommon();
					string tempStr;

					localeId = geoLocaleLookup.GetLocaleId(localeId);
					//Write CountryRegion
					tempStr = geoCountryRegion.GetByKey(geoData.CountryId, localeId);
					geoInfo.CountryRegionDisplayName = String.IsNullOrEmpty(tempStr) ? null : tempStr;
					tempStr = geoCRCodeLookup.GetByKey(geoData.CountryId);
					geoInfo.CountryRegionCode = String.IsNullOrEmpty(tempStr) ? null : tempStr;

					// Write StateProvince
					if (geoData.RegionCodesId != 0)
					{
						tempStr = geoStateProvince.GetByKey(geoData.RegionCodesId, localeId);
						geoInfo.StateProvinceDisplayName = String.IsNullOrEmpty(tempStr) ? null : tempStr;
						tempStr = geoSPCodeLookup.GetByKey(geoData.RegionCodesId);
						geoInfo.StateProvinceCode = String.IsNullOrEmpty(tempStr) ? null : tempStr;
					}

					// Write City
					if (geoData.CitiesId != 0)
					{
						tempStr = geoCity.GetByKey(geoData.CitiesId, localeId);
						geoInfo.CityDisplayName = String.IsNullOrEmpty(tempStr) ? null : tempStr;
					}

					// Write IsEU
					geoInfo.IsEuropeanUnion = geoData.IsEU;

					// now write the mappoint ID and parent ID
					if (geoData.CitiesId != 0)
					{
						geoInfo.MapPointId = geoData.CitiesId;
						if (geoData.RegionCodesId != 0)
						{
							geoInfo.MapPointIdParent = geoData.RegionCodesId;
						}
						else
						{
							geoInfo.MapPointIdParent = geoData.CountryId;
						}
					}
					else if (geoData.RegionCodesId != 0)
					{
						geoInfo.MapPointId = geoData.RegionCodesId;
						geoInfo.MapPointIdParent = geoData.CountryId;
					}
					else
					{
						geoInfo.MapPointId = geoData.CountryId;
						geoInfo.MapPointIdParent = -1;
					}

					/* Original logic is as following:
					int levelsWritten = 0;
					if (geoData.CitiesId != 0)
					{
					    georesult.Append(geoData.CitiesId).Append(MSCOMGeoConfig.GeoResultDelimitor);
					    levelsWritten++;
					}
					if (geoData.RegionCodesId != 0)
					{
					    georesult.Append(geoData.RegionCodesId).Append(MSCOMGeoConfig.GeoResultDelimitor);
					    levelsWritten++;
					}
					if (levelsWritten < 2)
					{
					    georesult.Append(geoData.CountryId).Append(MSCOMGeoConfig.GeoResultDelimitor);
					    levelsWritten++;
					}
					if (levelsWritten == 1)
					    georesult.Append("-1").Append(MSCOMGeoConfig.GeoResultDelimitor); //If parentID is null then return -1
					*/

					// Latitude & Longitude
					if (geoData.PositionId != 0)
					{
						GeoPositionData pos;
						if (geoPosition.GeoPositionDataById.TryGetValue(geoData.PositionId, out pos))
						{
							geoInfo.Latitude = pos.Latitude;
							geoInfo.Longitude = pos.Longitude;
						}
					}

					// Time Zone							
					if (geoData.TimeZonesId != 0)
					{
						GeoTimeZoneData tzData = geoTimeZone.GetByKey(geoData.TimeZonesId, localeId);
						if (tzData != null)
						{
							geoInfo.StandardTimeZoneOffset = Helper.ReadDoubleValue(tzData.StZoneOffSet);
                            geoInfo.DstTimeZoneOffset = Helper.ReadDoubleValue(tzData.DstZoneOffSet);
							geoInfo.DstStartTimeUniversal = Helper.ReadDateTimeValue(tzData.DstStartTime);
							geoInfo.DstEndTimeUniversal = Helper.ReadDateTimeValue(tzData.DstEndTime);
							tempStr = tzData.StName;
							geoInfo.TimeZoneDisplayName = String.IsNullOrEmpty(tempStr) ? null : tempStr;
							tempStr = tzData.DstName;
							geoInfo.DstTimeZoneDisplayName = String.IsNullOrEmpty(tempStr) ? null : tempStr;
						}
					}

					// Postcal code	
					if (zipCodeMode == 1)
						tempStr = geoData.ZipCodeString;
					else if (zipCodeMode == 2)
						tempStr = geoData.ZipCodeStringPlus;
					else
						tempStr = null;
					geoInfo.PostalCodeInfo = String.IsNullOrEmpty(tempStr) ? null : tempStr;
				}
			}
			catch
			{
				geoInfo = null;
			}
			return geoInfo;
		}         
      
        /// <summary>
        /// Given a localId (in string), return its language
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public string GetLanguageById(int localeId)
        {
            return geoLanguageLookup.GetLanguage(localeId);
        }

        /// <summary>
        /// Get a localeId by given language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public string GetLocaleIdByLanguage(string language)
        {
            return geoLocaleLookup.GetLocaleId(language);
        }

		public void GetGeoInfoByMappointId(int id, string localeId, int zipCodeMode, StringBuilder geoResult)
		{
			try
			{
				GeoData geoData = null;
				if (!geoIndex.GeoDataByID.TryGetValue(id, out geoData))
				{
                    geoResult.Append(MSCOMGeoConfig.SocketNotFound);
				}
				else
				{
					localeId = geoLocaleLookup.GetLocaleId(localeId);
					//Write CountryRegion
                    geoResult.Append(geoCountryRegion.GetByKey(geoData.CountryId, localeId));
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
                    geoResult.Append(geoCRCodeLookup.GetByKey(geoData.CountryId));
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);

					// Write StateProvince
					if (geoData.RegionCodesId != 0)
					{
                        geoResult.Append(geoStateProvince.GetByKey(geoData.RegionCodesId, localeId));
                        geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
                        geoResult.Append(geoSPCodeLookup.GetByKey(geoData.RegionCodesId));
					}
					else
                        geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);

					// Write City
					if (geoData.CitiesId != 0)
                        geoResult.Append(geoCity.GetByKey(geoData.CitiesId, localeId));
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
					
					// Write IsEU
                    geoResult.Append(geoData.IsEU ? 1 : 0);
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);

					// now write the mappoint ID and parent ID
					int levelsWritten = 0;
					if (geoData.CitiesId != 0)
					{
                        geoResult.Append(geoData.CitiesId).Append(MSCOMGeoConfig.GeoResultDelimiter);
						levelsWritten++;
					}
					if (geoData.RegionCodesId != 0)
					{
                        geoResult.Append(geoData.RegionCodesId).Append(MSCOMGeoConfig.GeoResultDelimiter);
						levelsWritten++;
					}
					if (levelsWritten < 2)
					{
                        geoResult.Append(geoData.CountryId).Append(MSCOMGeoConfig.GeoResultDelimiter);
						levelsWritten++;
					}
					if (levelsWritten == 1)
                        geoResult.Append("-1").Append(MSCOMGeoConfig.GeoResultDelimiter); //If parentID is null then return -1

					if (geoData.PositionId != 0)
					{
						GeoPositionData pos;
						if (geoPosition.GeoPositionDataById.TryGetValue(geoData.PositionId, out pos))
                            geoResult.Append(pos.Latitude).Append(MSCOMGeoConfig.GeoResultDelimiter).Append(pos.Longitude);
						else
                            geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
					}
					else
                        geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);

                    GeoTimeZoneData tzData = null;
					if (geoData.TimeZonesId != 0)
					{
						tzData = geoTimeZone.GetByKey(geoData.TimeZonesId, localeId);
						if (tzData != null)
						{
                            // For V1 only, the two fields of offset need to be int
                            // V2 client API will not read these two fields at all.
                            // Note to future developers: once Geo V1 is all upgraded, we can remove v1_region below, and can utilize these two fields for other purposes. 
                            #region v1_region
                            double temp;
                            if (!string.IsNullOrEmpty(tzData.StZoneOffSet))                                
                            {
                                double.TryParse(tzData.StZoneOffSet, out temp);
                                geoResult.Append((int)temp);
                            }
                            geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
                            if (!string.IsNullOrEmpty(tzData.DstZoneOffSet))
                            {
                                double.TryParse(tzData.DstZoneOffSet, out temp);
                                geoResult.Append((int)temp);
                            }
                            geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
                            #endregion

                            geoResult.Append(tzData.DstStartTime).Append(MSCOMGeoConfig.GeoResultDelimiter);
                            geoResult.Append(tzData.DstEndTime).Append(MSCOMGeoConfig.GeoResultDelimiter);
                            geoResult.Append(tzData.StName).Append(MSCOMGeoConfig.GeoResultDelimiter);
                            geoResult.Append(tzData.DstName);		
						}
						else
                            geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter);
					}
					else
                        geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter).Append(MSCOMGeoConfig.GeoResultDelimiter);
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
										
					if (zipCodeMode == 1)
                        geoResult.Append(geoData.ZipCodeString);
					else if (zipCodeMode == 2)
                        geoResult.Append(geoData.ZipCodeStringPlus);
                    geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);

                    // This is for V2 and above to read the offsets
                    if (tzData != null)
                    {
                        geoResult.Append(tzData.StZoneOffSet).Append(MSCOMGeoConfig.GeoResultDelimiter);
                        geoResult.Append(tzData.DstZoneOffSet);
                    }
                    else
                    {
                        geoResult.Append(MSCOMGeoConfig.GeoResultDelimiter);
                    }
				}
			}
            catch { geoResult.Append(MSCOMGeoConfig.SocketException); }			
		}

		/// <summary>with a given GeoID and localeID, return the detailed geoinformation</summary>
		public void GetGeoInfoByIP(string ip, string localeId, int zipCodeMode, StringBuilder geoResult)
		{
			// fengj: internally not checking geoResult.
			try
			{				
				int geoID = Lookup(ip);
				if (geoID < 0)
					geoResult.Append(MSCOMGeoConfig.SocketNotFound);
				else
				{
					GetGeoInfoByMappointId(geoID, localeId, zipCodeMode, geoResult);
				}
			}
			catch { geoResult.Append(MSCOMGeoConfig.SocketException); }			
		}

		/// <summary>
		/// Returns the detailed geoinformation with given IP and localeID.
		/// </summary>
		/// <param name="ip">The ip.</param>
		/// <param name="localeID">The locale ID.</param>
		/// <param name="zipCodeMode">The zip code mode.</param>
		/// <returns>
		/// The GeoInfoCommon object containing geo information. Can be null.
		/// </returns>
		/// <remarks>New API for high speed in process access without serialization to string.</remarks>
		public GeoInfoCommon GetGeoInfoByIP(string ip, string localeId, int zipCodeMode)
		{
			GeoInfoCommon geoInfo;
			try
			{
				int geoID = Lookup(ip);
				if (geoID < 0)
					geoInfo = null;
				else
				{
					geoInfo = GetGeoInfoByMappointId(geoID, localeId, zipCodeMode);
				}
			}
			catch { geoInfo = null; }
			return geoInfo;
        }

        public void GetGeoInfoByPostalCode(string postalCode, string countryCode, string localeId, int zipCodeMode, StringBuilder geoResult)
        {
			// fengj: internally not checking geoResult.
			try
            {
                int geoID = geoPostalCodeLookup.LookupPostalCode(postalCode, countryCode);
                if (geoID < 0)
                    geoResult.Append(MSCOMGeoConfig.SocketNotFound);
                else
                {
                    GetGeoInfoByMappointId(geoID, localeId, zipCodeMode, geoResult);
                }
            }
            catch { geoResult.Append(MSCOMGeoConfig.SocketException); }			
        }

		/// <summary>
		/// Returns the detailed geoinformation with given postacal code, country code and localeID.
		/// </summary>
		/// <param name="postalCode">postalCode</param>
		/// <param name="countryCode">CountryCode</param>
		/// <param name="localeID">The locale ID.</param>
		/// <param name="zipCodeMode">The zip code mode.</param>
		/// <returns>
		/// The GeoInfoCommon object containing geo information. Can be null.
		/// </returns>
		/// <remarks>New API for high speed in process access without serialization to string.</remarks>
		public GeoInfoCommon GetGeoInfoByPostalCode(string postalCode, string countryCode, string localeId, int zipCodeMode)
        {
			GeoInfoCommon geoInfo;
            try
            {
                int geoID = geoPostalCodeLookup.LookupPostalCode(postalCode, countryCode);
                if (geoID < 0)
                    geoInfo = null;
                else
                {
					geoInfo = GetGeoInfoByMappointId(geoID, localeId, zipCodeMode);
                }
            }
            catch { geoInfo = null; }
			return geoInfo;
        }

        //=====================================================================
        //  Method: Write
        //
        /// <summary>
        /// Writes the Geo data to the specified writer
        /// </summary>  
        //=====================================================================

        public void Write(BinaryWriter geoDataWriter)
        {
            this.geoDataVersion.SaveToFile(geoDataWriter);
            if (MSCOMGeoConfig.IsIPv4Enabled != false)
            {
                this.geoInfoData.SaveToFile(geoDataWriter);
            }
            if (MSCOMGeoConfig.IsIPv6Enabled != false)
            {
                this.geoInfoDataIPv6.SaveToFile(geoDataWriter);
            }

            this.reservedLookupIPv4.SaveToFile(geoDataWriter);
            this.reservedLookupIPv6.SaveToFile(geoDataWriter);

            this.geoIndex.SaveToFile(geoDataWriter);
            this.geoCountryRegion.SaveToFile(geoDataWriter);
            this.geoStateProvince.SaveToFile(geoDataWriter);
            this.geoCity.SaveToFile(geoDataWriter);
            this.geoTimeZone.SaveToFile(geoDataWriter);
            this.geoPosition.SaveToFile(geoDataWriter);
            this.geoCRCodeLookup.SaveToFile(geoDataWriter);
            this.geoSPCodeLookup.SaveToFile(geoDataWriter);
            this.geoLocaleLookup.SaveToFile(geoDataWriter);
            this.geoLanguageLookup.SaveToFile(geoDataWriter);
            this.geoPostalCodeLookup.SaveToFile(geoDataWriter);
            this.geoEntityRelation.SaveToFile(geoDataWriter); // this one must be at the end!!!
        }

        //=====================================================================
        //  Method: Read
        //
        /// <summary>
        /// Reads the Geo data to the specified reader
        /// </summary>  
        //=====================================================================

        public void Read(BinaryReader geoDataReader)
        {
            geoDataVersion.LoadFromFile(geoDataReader);
            if (MSCOMGeoConfig.IsIPv4Enabled != false)
            {
                this.geoInfoData.LoadFromFile(geoDataReader);
            }
            if (MSCOMGeoConfig.IsIPv6Enabled != false)
            {
                this.geoInfoDataIPv6.LoadFromFile(geoDataReader);
            }

            this.reservedLookupIPv4.LoadFromFile(geoDataReader);
            this.reservedLookupIPv6.LoadFromFile(geoDataReader);

            geoIndex.LoadFromFile(geoDataReader);
            geoCountryRegion.LoadFromFile(geoDataReader);
            geoStateProvince.LoadFromFile(geoDataReader);
            geoCity.LoadFromFile(geoDataReader);
            geoTimeZone.LoadFromFile(geoDataReader);
            geoPosition.LoadFromFile(geoDataReader);
            geoCRCodeLookup.LoadFromFile(geoDataReader);
            geoSPCodeLookup.LoadFromFile(geoDataReader);
            geoLocaleLookup.LoadFromFile(geoDataReader);
            geoLanguageLookup.LoadFromFile(geoDataReader);
            geoPostalCodeLookup.LoadFromFile(geoDataReader);
            geoEntityRelation.LoadFromFile(geoDataReader);

            geoEntityRelation.PopulateLocalizedNameForNodes(
                geoLanguageLookup, 
                geoCountryRegion, 
                geoStateProvince, 
                geoCity
                );
        }

        public void Clear()
        {
            if (MSCOMGeoConfig.IsIPv4Enabled != false)
            {
                if (this.geoInfoData != null)
                {
                    this.geoInfoData.Clear();
                }
            }

            if (MSCOMGeoConfig.IsIPv6Enabled != false)
            {
                if (this.geoInfoDataIPv6 != null)
                {
                    this.geoInfoDataIPv6.Clear();
                }
            }

            if (this.reservedLookupIPv4 != null)
            {
                this.reservedLookupIPv4.Clear();
            }

            if (this.reservedLookupIPv6 != null)
            {
                this.reservedLookupIPv6.Clear();
            }


            if (geoDataVersion != null)
            {
                this.geoDataVersion.Clear();
            }

            if (geoIndex != null)
            {
                this.geoIndex.Clear();
            }

            if (geoCountryRegion != null)
            {
                this.geoCountryRegion.Clear();
            }

            if (geoStateProvince != null)
            {
                this.geoStateProvince.Clear();
            }

            if (geoCity != null)
            {
                this.geoCity.Clear();
            }

            if (geoTimeZone != null)
            {
                this.geoTimeZone.Clear();
            }

            if (geoPosition != null)
            {
                this.geoPosition.Clear();
            }

            if (geoCRCodeLookup != null)
            {
                this.geoCRCodeLookup.Clear();
            }

            if (geoSPCodeLookup != null)
            {
                this.geoSPCodeLookup.Clear();
            }

            if (geoLocaleLookup != null)
            {
                this.geoLocaleLookup.Clear();
            }

            if (geoLanguageLookup != null)
            {
                this.geoLanguageLookup.Clear();
            }

            if (geoPostalCodeLookup != null)
            {
                this.geoPostalCodeLookup.Clear();
            }

            if (geoEntityRelation != null)
            {
                this.geoEntityRelation.Clear();
            }

            if (geoEntityRelation != null)
            {
                this.geoEntityRelation.Clear();
            }
        }


        #region methods for GetCountryRegionList and GetChildrenList
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID">if negative, then get all countries. Otherwise, get children.</param>
        /// <returns></returns>
        public List<GeoEntityNode> GetGeoEntityChildrenList (int parentId)
        {
            GeoEntityNode parent = geoEntityRelation.GetEntityNodeByID(parentId);
            if (parent != null)
                return parent.Children;
            else
                return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Mappoint ID for Country and StateProvince</param>
        /// <param name="level">1: Country, 2: State. 3: city. this is for faster search for ID</param>
        /// <param name="localeId">make it a string so it is easy to use. it is "9" for english</param>
        /// <returns>localizedName</returns>
        internal static string GetGeoEntityNameByIDLocaleLevel(int id, GeoEntityLevel level, string localeId, GeoCountryRegion cr, GeoStateProvince sp, GeoCity c)
        {
            switch (level)
            {
                case GeoEntityLevel.CountryRegion:
                    return cr.GetByKey(id, localeId);
                case GeoEntityLevel.StateProvince:
                    return sp.GetByKey(id, localeId);
                case GeoEntityLevel.City:
                    return c.GetByKey(id, localeId);
                default:
                    throw new ArgumentException("level is out of allowable range: " + level);
            }
        }

        /// <summary>
		/// Get all countries in a list using given language
		/// </summary>
		/// <param name="language">the language</param>
		/// <returns>a string for socket transfer</returns>
        public void GetCountryRegionList(string language, StringBuilder result)
        {
            try
            {
                List<GeoEntityNode> list = GetGeoEntityChildrenList(-1);
                if (list == null)
                {
                    result.Append(MSCOMGeoConfig.SocketNotFound);
                    return;
                }
                string localeId = this.geoLocaleLookup.GetLocaleId(language);                
                result.Append(list.Count).Append(MSCOMGeoConfig.GeoResultDelimiter);
                result.Append(localeId).Append(MSCOMGeoConfig.GeoResultDelimiter);

                int intLocalId = Int32.Parse(localeId, CultureInfo.InvariantCulture);
                int pos = geoEntityRelation.GetLocalizedNamePositionByLocalId(intLocalId);

                foreach (GeoEntityNode node in list)
                {
                    result.Append(node.Id).Append(MSCOMGeoConfig.GeoResultDelimiter);
                    result.Append(node.Code).Append(MSCOMGeoConfig.GeoResultDelimiter);
                    //result.Append(GetGeoEntityNameByIDLocaleLevel(node.ID, node.Level, localeId)).Append(MSCOMGeoConfig.GeoResultDelimitor);
                    result.Append(node.LocalizedNames[pos]).Append(MSCOMGeoConfig.GeoResultDelimiter);
                    result.Append(node.IsEU ? '1' : '0').Append(MSCOMGeoConfig.GeoResultDelimiter);
                    result.Append(node.SupportPostalCode ? '1' : '0').Append(MSCOMGeoConfig.GeoResultDelimiter);
                }
            }
            catch
            {
                result.Append(MSCOMGeoConfig.SocketException); 
            }
        }

        /// <summary>
        /// Get all countries in a list using given language
        /// </summary>
        /// <param name="language">the language</param>
        /// <returns>List of CountryRegion</returns>
        public List<CountryRegion> GetCountryRegionList(string language)
        {
            try
            {
                List<GeoEntityNode> list = GetGeoEntityChildrenList(-1);
                if (list == null || list.Count == 0)
                    return null;

                string localeId = this.geoLocaleLookup.GetLocaleId(language);
                int intLocalId = Int32.Parse(localeId, CultureInfo.InvariantCulture);
                int pos = geoEntityRelation.GetLocalizedNamePositionByLocalId(intLocalId);

                List<CountryRegion> resultList = new List<CountryRegion>(list.Count);
                foreach (GeoEntityNode node in list)
                {
                    CountryRegion result = new CountryRegion(node.Id, node.Code);
                    result.DisplayLanguage = language;
                    //result.DisplayName = GetGeoEntityNameByIDLocaleLevel(node.ID, node.Level, languageId);
                    result.DisplayName = node.LocalizedNames[pos];
                    result.IsEU = node.IsEU;
                    result.SupportPostalCode = node.SupportPostalCode;
                    resultList.Add(result);
                }
              
                return resultList;

            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Get all children Geo entities of a given Geo ID (in English)
        /// </summary>
        /// <param name="id">ID of the Geo Entity</param>
        /// <param name="languageId"></param>
        /// <returns>a string for socket transfer</returns>
        public void GetChildListFromId(int id, int languageId, StringBuilder result)
        {
            try
            {
                List<GeoEntityNode> list = GetGeoEntityChildrenList(id);
                if (list == null)
                {
                    result.Append(MSCOMGeoConfig.SocketNotFound);
                    return;
                }
                //string localeId = languageId.ToString(CultureInfo.InvariantCulture);                
                result.Append(list.Count).Append(MSCOMGeoConfig.GeoResultDelimiter);
                result.Append(GetLanguageById(languageId)).Append(MSCOMGeoConfig.GeoResultDelimiter);
                                
                int pos = geoEntityRelation.GetLocalizedNamePositionByLocalId(languageId);

                foreach (GeoEntityNode node in list)
                {
                    result.Append(node.Id).Append(MSCOMGeoConfig.GeoResultDelimiter);
                    result.Append(node.Code).Append(MSCOMGeoConfig.GeoResultDelimiter);
                    result.Append((short)node.Level).Append(MSCOMGeoConfig.GeoResultDelimiter);
                    //result.Append(GetGeoEntityNameByIDLocaleLevel(node.ID, node.Level, localeId)).Append(MSCOMGeoConfig.GeoResultDelimitor);
                    result.Append(node.LocalizedNames[pos]).Append(MSCOMGeoConfig.GeoResultDelimiter);
                }
            }
            catch
            {
                result.Append(MSCOMGeoConfig.SocketException); 
            }
        }

        /// <summary>
        /// Get all children Geo entities of a given Geo ID (in English)
        /// </summary>
        /// <param name="id">ID of the Geo Entity</param>
        /// <param name="languageId"></param>
        /// <returns>List of GeoEntity</returns>
        public List<GeoEntity> GetChildListFromId(int id, int languageId)
        {
            try
            {
                List<GeoEntityNode> list = GetGeoEntityChildrenList(id);
                if (list == null || list.Count == 0)
                    return null;

                int pos = geoEntityRelation.GetLocalizedNamePositionByLocalId(languageId);
                string displayLanguage = GetLanguageById(languageId);

                List<GeoEntity> resultList = new List<GeoEntity>(list.Count);
                foreach (GeoEntityNode node in list)
                {
                    GeoEntity result = null;
                    if (node.Level == GeoEntityLevel.StateProvince)
                    {
                        result = new StateProvince(node.Id, node.Code, id, languageId);
                    }
                    else //(level == GeoEntityLevel.City)
                    {
                        result = new City(node.Id, node.Code, id, languageId);
                    }
                    result.DisplayLanguage = displayLanguage;
                    //result.DisplayName = GetGeoEntityNameByIDLocaleLevel(node.ID, node.Level, languageId.ToString());
                    result.DisplayName = node.LocalizedNames[pos];
                    resultList.Add(result);
                }
             
                return resultList;
            }
            catch
            {
                return null;
            }
        }
        public int GetLocalizedNamePositionByLocalId(int localeId)
        {
            return geoEntityRelation.GetLocalizedNamePositionByLocalId(localeId);
        }
        #endregion
    }
}
