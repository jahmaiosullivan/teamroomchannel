using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.IT.Geo.Legacy.GeoCommon;
using Microsoft.IT.Geo.Legacy.GeoLocationApi;
using Microsoft.IT.Geo.Legacy.GeoLocationApi.Common;
using MSCOMGeoSystem.Common;
using System.IO;
using System.Globalization;

namespace Microsoft.MSCOM.Geo.GeoLocationAPI
{
	/// <summary>
	/// The API providing high speed in-process Geo location retrieval without accessing windows service.
	/// </summary>	
	internal static class HighSpeedAPI
	{			
		static GeoInfoLookup _geoInfoLookup;
        static FileSystemWatcher _fileWatcher;
        static DateTime _lastUpdateDateTime = new DateTime(2000, 1, 1);
        static object _syncRoot = new object();
        static byte[] _datahash = null;

		/// <summary>
		/// Initializes the <see cref="HighSpeedAPI"/> class.
		/// </summary>
		/// <remarks>
		/// Loads the data file from disk to memory. Not saving the file to disk.
		/// Will retry to load from back data file if the loading of data file failed.
		/// </remarks>
		static HighSpeedAPI()
        {
            if (MSCOMGeoConfig.HighSpeedApiEnabled)
            {
                try
                {
                    _geoInfoLookup = new GeoInfoLookup();
                    _datahash = Helper.ComputerHash(GeoInfoLookup.FileName);
                    if (_geoInfoLookup != null)
                        _lastUpdateDateTime = DateTime.Now;
                }
                catch
                {
                    //EventLogWriter.WriteEntry(EventType.HighSpeed, "HighSpeedAPI:Initial load: Error occurred in loading the data", System.Diagnostics.EventLogEntryType.Error);                    
                    try
                    {
                        _geoInfoLookup = new GeoInfoLookup();                        
                        _datahash = Helper.ComputerHash(GeoInfoLookup.BackupFileName);
                    }
                    catch
                    {
                        //EventLogWriter.WriteEntry(EventType.HighSpeed, "HighSpeedAPI:Initial load: Failed in loading data from backup file. System will not be able to run.", System.Diagnostics.EventLogEntryType.Error);                        
                    }                
                }                                
                _fileWatcher = new FileSystemWatcher();  
                _fileWatcher.Path = MSCOMGeoConfig.DBDataPath;
                _fileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                _fileWatcher.Filter = GeoInfoLookup.FileNameWithoutPath;
                _fileWatcher.Changed += new FileSystemEventHandler(onChanged);
                _fileWatcher.Created += new FileSystemEventHandler(onChanged);
                _fileWatcher.EnableRaisingEvents = true;
            }
        }

        private static void onChanged(object source, FileSystemEventArgs e)
        {
            lock (_syncRoot)
            {
                if (((TimeSpan)DateTime.Now.Subtract(_lastUpdateDateTime)).TotalMilliseconds < MSCOMGeoConfig.TcpListenerTimeoutForSync)
                    return; // duplicate events
                else
                    _lastUpdateDateTime = DateTime.Now;
            }            
            try
            {
                byte[] newDataHash = Helper.ComputerHash(GeoInfoLookup.FileName);
                if (!Helper.IsHashSame(newDataHash, _datahash))
                {
                    //EventLogWriter.WriteEntry(EventType.HighSpeed, "HighSpeedAPI:onChanged: Begin to load a new data file", System.Diagnostics.EventLogEntryType.Information);                    
					_geoInfoLookup = new GeoInfoLookup();
                    _datahash = newDataHash;
                }
                else
                    return;
            }
            catch
            {
                //EventLogWriter.WriteEntry(EventType.HighSpeed, "HighSpeedAPI:onChanged: Error occurred in loading the data from refreshed file", System.Diagnostics.EventLogEntryType.Error);                
                return;
            }            
        }

        internal static GeoInfo GetGeoInfo(string postalCode, string countryCode, string language, GeoPostalCodeRangeMode postalcodeMode)
        {
            if (_geoInfoLookup == null)
                return null;            
            GeoInfoCommon geoInfoCommon = _geoInfoLookup.GetGeoInfoByPostalCode(postalCode, countryCode, language, (int)postalcodeMode);
            if (geoInfoCommon == null)
                return null;
            else
                return new GeoInfo(geoInfoCommon); 
        }

		/// <summary>
		/// Gets the geo info.
		/// </summary>
		/// <param name="key">Mappoint ID or IP</param>
		/// <param name="language">The language.</param>
		/// <param name="postalcodeMode">The postalcode mode.</param>
		/// <returns>The GeoInfo object. </returns>
		internal static GeoInfo GetGeoInfo(string key, string language, GeoPostalCodeRangeMode postalcodeMode)
		{
            if (_geoInfoLookup == null)
                return null;
			
			int mappointID = -1;			
			GeoInfoCommon geoInfoCommon;
			if (Int32.TryParse(key, out mappointID))
			{
				geoInfoCommon = _geoInfoLookup.GetGeoInfoByMappointId(mappointID, language, (int)postalcodeMode);
			}
			else
			{
				geoInfoCommon = _geoInfoLookup.GetGeoInfoByIP(key, language, (int)postalcodeMode);
			}
            if (geoInfoCommon == null)
                return null;
            else
                return new GeoInfo(geoInfoCommon);
		}

		/// <summary>
		/// Gets the child list from id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <param name="languageId">The language id.</param>
		/// <returns>List of GeoEntities. </returns>
        internal static List<GeoEntity> GetChildListFromId(int id, int languageId)
        {
            if (_geoInfoLookup == null)
                return null;

            string displayLanguage = _geoInfoLookup.GetLanguageById(languageId);
            List<GeoEntityNode> nodes = _geoInfoLookup.GetGeoEntityChildrenList(id);
            if (nodes == null || nodes.Count == 0)
                return null;

            int pos = _geoInfoLookup.GetLocalizedNamePositionByLocalId(languageId);

            List<GeoEntity> resultList = new List<GeoEntity>(nodes.Count);
            foreach (GeoEntityNode node in nodes)
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
                //result.DisplayName = _geoInfoLookup.GetGeoEntityNameByIDLocaleLevel(node.ID, node.Level, languageId.ToString());
                result.DisplayName = node.LocalizedNames[pos];
                resultList.Add(result);                
            }
            return resultList;
        }

		/// <summary>
		/// Get all countries in a list using given language
		/// </summary>
		/// <param name="language">the language</param>
		/// <returns>a list of countries</returns>
        internal static List<CountryRegion> GetCountryRegionList(string language)
        {
            if (_geoInfoLookup == null)
                return null;
                        
            List<GeoEntityNode> nodes = _geoInfoLookup.GetGeoEntityChildrenList(-1);
            if (nodes == null || nodes.Count == 0)
                return null;

            string languageId = _geoInfoLookup.GetLocaleIdByLanguage(language);

            int intLocalId = Int32.Parse(languageId, CultureInfo.InvariantCulture);
            int pos = _geoInfoLookup.GetLocalizedNamePositionByLocalId(intLocalId);

            List<CountryRegion> resultList = new List<CountryRegion>(nodes.Count);
            foreach (GeoEntityNode node in nodes)
            {
                CountryRegion result = new CountryRegion(node.Id, node.Code);                
                result.DisplayLanguage = language;
                //result.DisplayName = _geoInfoLookup.GetGeoEntityNameByIDLocaleLevel(node.ID, node.Level, languageId);
                result.DisplayName = node.LocalizedNames[pos];
                result.IsEU = node.IsEU;
                result.SupportPostalCode = node.SupportPostalCode;
                resultList.Add(result);
            }
            return resultList;
        }
	}
}
