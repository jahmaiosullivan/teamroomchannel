using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoCommon
{	
	public class GeoData
	{
		// use it as class instead of struct. As there is not much perf and storage difference after testing, and it can be called by reference.
		private Int32 countryId;
		private Int32 regionCodesId;
		private Int32 citiesId;
		private Int32 timeZonesId;
		private Int32 positionId;
		private bool isEU;
		private string zipCodeString;
		private string zipCodeStringPlus;
		public GeoData(){}
        public GeoData(int countryId, int regionCodesId, int citiesId, int timeZonesId, int positionId, int isEu, string zipCode, string zipCodePlus)
		{
			this.countryId = countryId;
            this.regionCodesId = regionCodesId;
            this.citiesId = citiesId;
            this.timeZonesId = timeZonesId;
            this.positionId = positionId;
            this.isEU = (isEu == 1);
            this.zipCodeString = string.IsNullOrEmpty(zipCode) ? null : zipCode;
            this.zipCodeStringPlus = string.IsNullOrEmpty(zipCodePlus) ? null : zipCodePlus;
		}
		public string ZipCodeString
		{
			get { return zipCodeString; }
			set { zipCodeString = value; }
		}
		public string ZipCodeStringPlus
		{
			get { return zipCodeStringPlus; }
			set { zipCodeStringPlus = value; }
		}
		public bool IsEU
		{
			get{return isEU;}
			set{isEU = value;}
		}
		public Int32 PositionId
		{
			get{return positionId;}
			set{positionId = value;}
		}
		public Int32 CountryId
		{
			get	{return countryId;}
			set { countryId = value; }
		}
		public Int32 RegionCodesId
		{
			get { return regionCodesId; }
			set { regionCodesId = value; }
		}
		public Int32 CitiesId
		{
			get { return citiesId; }
			set { citiesId = value; }
		}
		public Int32 TimeZonesId
		{
			get { return timeZonesId; }
			set { timeZonesId = value; }
		}		
	}

	class GeoDataVersion : GeoDataFeedBase
	{
		private string dataVersion;
		public string DataVersion
		{
			get { return dataVersion; }
		}
		public GeoDataVersion()
		{			
			dbCapacitySP = null;
			dbDataSP = Helper.GetConfigureString("GeoDataVersionGetForSocket", "GeoDataVersionGetForSocket");
		}


		protected override void PopulateDataFromFile(BinaryReader r)
		{
			dataVersion = r.ReadString();
		}

		protected override void WriteDataToFile(BinaryWriter w)
		{
			w.Write(dataVersion);
		}

        public override void Clear()
        {
        }
	}

	class GeoIndex : GeoDataFeedBase
	{
		public Dictionary<Int32, GeoData> GeoDataByID;
		public GeoIndex()
		{
			dbCapacitySP = Helper.GetConfigureString("GeoIndexCountGetForSocket", "GeoIndexCountGetForSocket");
			dbDataSP = Helper.GetConfigureString("GeoIndexSelectForSocket", "GeoIndexSelectForSocket");
		}
        
		protected override void PopulateDataFromFile(BinaryReader r)
		{		
			int listCount = r.ReadInt32();
			GeoDataByID = new Dictionary<Int32, GeoData>(listCount);
			Int32 val1;
			Int32 val2;
			Int32 val3;
			Int32 val4;
			Int32 val5;
			Int32 val6;
			Int32 val7;
			string val8;
			string val9;

			for (int i = 0; i < listCount; i++)
			{
				val1 = r.ReadInt32();
				val2 = r.ReadInt32();
				val3 = r.ReadInt32();
				val4 = r.ReadInt32();
				val5 = r.ReadInt32();
				val6 = r.ReadInt32();
				val7 = r.ReadInt32();
				val8 = r.ReadString();
				val9 = r.ReadString();
				GeoDataByID.Add(val1, new GeoData(val2, val3, val4, val5, val6, val7, val8, val9));
			}			
		}

		override protected void WriteDataToFile(BinaryWriter w)
		{			
			w.Write(GeoDataByID.Count);

			foreach (KeyValuePair<Int32, GeoData> entry in GeoDataByID)
			{
				w.Write(entry.Key);
				w.Write(entry.Value.CountryId);
				w.Write(entry.Value.RegionCodesId);
				w.Write(entry.Value.CitiesId);
				w.Write(entry.Value.TimeZonesId);
				w.Write(entry.Value.PositionId);
				w.Write(entry.Value.IsEU ? 1:0);
				w.Write(entry.Value.ZipCodeString == null ? "" : entry.Value.ZipCodeString);
				w.Write(entry.Value.ZipCodeStringPlus == null ? "" : entry.Value.ZipCodeStringPlus);
			}				
		}

        public override void Clear()
        {
            if (this.GeoDataByID != null)
            {
                this.GeoDataByID.Clear();
            }
        }
	}

	struct GeoPositionData
	{
		public double Longitude;
		public double Latitude;
		public GeoPositionData(double longitude, double latitude)
		{
			Longitude = longitude;
			Latitude = latitude;
		}
	}

	class GeoPosition : GeoDataFeedBase
	{
		public Dictionary<Int32, GeoPositionData> GeoPositionDataById;
		public GeoPosition()
		{
			dbCapacitySP = Helper.GetConfigureString("GeoPositionCountGetForSocket", "GeoPositionCountGetForSocket");
			dbDataSP = Helper.GetConfigureString("GeoPositionSelectForSocket", "GeoPositionSelectForSocket");
		}
		

		protected override void PopulateDataFromFile(BinaryReader r)
		{
			int listCount = r.ReadInt32();
			GeoPositionDataById= new Dictionary<Int32, GeoPositionData>(listCount);
			Int32 id;
			double val1;
			double val2;
			for (int i = 0; i < listCount; i++)
			{
				id = r.ReadInt32();
				val1 = r.ReadDouble();
				val2 = r.ReadDouble();
				GeoPositionDataById.Add(id, new GeoPositionData(val1, val2));
			}
		}

		protected override void WriteDataToFile(BinaryWriter w)
		{
			w.Write(GeoPositionDataById.Count);

			foreach (KeyValuePair<Int32, GeoPositionData> entry in GeoPositionDataById)
			{
				w.Write(entry.Key);
				w.Write(entry.Value.Longitude);
				w.Write(entry.Value.Latitude);				
			}				
		}

        public override void Clear()
        {
            if (this.GeoPositionDataById != null)
            {
                this.GeoPositionDataById.Clear();
            }
        }
	}
		
	#region KeyLocaleValue classes
	abstract class GeoKeyLocaleValueDataFeedBase : GeoDataFeedBase
	{
		private static string keyLocaleSplitor = ":";		
		protected Dictionary<string, string> geoLookupTable;
#if UNITTEST
		public Dictionary<string, string> GeoLookupTable
		{
			get { return geoLookupTable; }	
		}
#endif

		public virtual string GetByKey(int id, string localeID)
		{
			string val = String.Empty;
			if (geoLookupTable.TryGetValue(id + keyLocaleSplitor + localeID, out val))
				return val;
			else
				return String.Empty;
				/*
			else if (localeID != MSCOMGeoConfig.DefaultLocale)
			{
				// fall back to english
				geoLookupTable.TryGetValue(id + keyLocaleSplitor + MSCOMGeoConfig.DefaultLocale, out val);			
			}
			return val;
				 */ 
		}
        
		protected override void PopulateDataFromFile(BinaryReader r)
		{			
			int listCount = r.ReadInt32();
			geoLookupTable = new Dictionary<string, string>(listCount);
			string key;
			string val;
			for (int i = 0; i < listCount; i++)
			{						
				key = r.ReadString();						
				val = r.ReadString();
				geoLookupTable.Add(key, val);
			}	
		}

		override protected void WriteDataToFile(BinaryWriter w)
		{			
			w.Write(geoLookupTable.Count);
			foreach (KeyValuePair<string, string> entry in geoLookupTable)
			{
				w.Write(entry.Key);
				w.Write(entry.Value);
			}
		}

        public override void Clear()
        {
            if (this.geoLookupTable != null)
            {
                this.geoLookupTable.Clear();
            }
        }
	}

	class GeoCountryRegion : GeoKeyLocaleValueDataFeedBase
	{		
		public GeoCountryRegion()
		{
			dbDataSP = Helper.GetConfigureString("GeoCountryRegionSelectForSocket", "GeoCountryRegionSelectForSocket");
			dbCapacitySP = Helper.GetConfigureString("GeoCountryRegionCountGetForSocket", "GeoCountryRegionCountGetForSocket");
		}
	}

	class GeoCity : GeoKeyLocaleValueDataFeedBase
	{		
		public GeoCity()
		{
			dbDataSP = Helper.GetConfigureString("GeoLocalizedCitySelectForSocket", "GeoLocalizedCitySelectForSocket");
			dbCapacitySP = Helper.GetConfigureString("GeoLocalizedCityCountGetForSocket", "GeoLocalizedCityCountGetForSocket");
		}
	}

	class GeoStateProvince : GeoKeyLocaleValueDataFeedBase
	{	
		public GeoStateProvince()
		{
			dbDataSP = Helper.GetConfigureString("GeoStateProvinceSelectForSocket", "GeoStateProvinceSelectForSocket");
			dbCapacitySP = Helper.GetConfigureString("GeoStateProvinceCountGetForSocket", "GeoStateProvinceCountGetForSocket");
		}
	}
		
	#endregion

	#region
	abstract class GeoKeyValueDataFeedBase : GeoDataFeedBase
	{		
		protected Dictionary<Int32, string> geoLookupTable;

		public virtual string GetByKey(int id)
		{
			string val = null;
			if (geoLookupTable.TryGetValue(id, out val))
				return val;
			else
				return string.Empty;
		}
        
		protected override void PopulateDataFromFile(BinaryReader r)
		{
			int listCount = r.ReadInt32();
			geoLookupTable = new Dictionary<Int32, string>(listCount);
			Int32 key;
			string val;
			for (int i = 0; i < listCount; i++)
			{
				key = r.ReadInt32();
				val = r.ReadString();
				geoLookupTable.Add(key, val);
			}
		}

		override protected void WriteDataToFile(BinaryWriter w)
		{
			w.Write(geoLookupTable.Count);
			foreach (KeyValuePair<Int32, string> entry in geoLookupTable)
			{
				w.Write(entry.Key);
				w.Write(entry.Value);
			}
		}

        public override void Clear()
        {
            if (this.geoLookupTable != null)
            {
                this.geoLookupTable.Clear();
            }
        }
	}

	class GeoCountryRegionCodeLookup : GeoKeyValueDataFeedBase
	{
		public GeoCountryRegionCodeLookup()
		{
			dbDataSP = Helper.GetConfigureString("GeoCountryRegionCodeSelectForSocket", "GeoCountryRegionCodeSelectForSocket");
			dbCapacitySP = Helper.GetConfigureString("GeoCountryRegionCodeCountGetForSocket", "GeoCountryRegionCodeCountGetForSocket");
		}
	}

	class GeoStateProvinceCodeLookup : GeoKeyValueDataFeedBase
	{
		public GeoStateProvinceCodeLookup()
		{
			dbDataSP = Helper.GetConfigureString("GeoStateProvinceCodeSelectForSocket", "GeoStateProvinceCodeSelectForSocket");
			dbCapacitySP = Helper.GetConfigureString("GeoStateProvinceCodeCountGetForSocket", "GeoStateProvinceCodeCountGetForSocket");
		}
	}

    /// <summary>
    /// This class is to lookup LocaleID with given language
    /// </summary>
	class GeoLocaleLookup : GeoDataFeedBase
	{
		private Dictionary<string, string> localeLookup;

        /// <summary>
        /// Language->LanguageId lookup. LocaleId is string type. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		public string GetLocaleId(string input)
		{
            if(string.IsNullOrEmpty(input))
                return MSCOMGeoConfig.DefaultLocale.ToString(CultureInfo.InvariantCulture);

			string val = null;
            if (localeLookup.TryGetValue(input.ToLower(CultureInfo.InvariantCulture), out val))
				return val;
			else
			{
				int dummy;
				if (Int32.TryParse(input, out dummy))
					return input; // caller might give localeId directly, which is a less case.
				else
                    return MSCOMGeoConfig.DefaultLocale.ToString(CultureInfo.InvariantCulture);
			}
		}
		public GeoLocaleLookup()
		{
			dbCapacitySP = Helper.GetConfigureString("GeoLocaleLookupCountGetForSocket", "GeoLocaleLookupCountGetForSocket");
			dbDataSP = Helper.GetConfigureString("GeoLocaleLookupSelectForSocket", "GeoLocaleLookupSelectForSocket");
		}
        
		protected override void PopulateDataFromFile(BinaryReader r)
		{
			int listCount = r.ReadInt32();
			localeLookup = new Dictionary<string, string>(listCount);
			string id;
			string val1;
			
			for (int i = 0; i < listCount; i++)
			{
				id = r.ReadString();
				val1 = r.ReadString();
				localeLookup.Add(val1, id);
			}
		}

		protected override void WriteDataToFile(BinaryWriter w)
		{
			w.Write(localeLookup.Count);

			foreach (KeyValuePair<string, string> entry in localeLookup)
			{
				w.Write(entry.Value);
				w.Write(entry.Key);				
			}
		}

        public override void Clear()
        {
            if (this.localeLookup != null)
            {
                this.localeLookup.Clear();
            }
        }
	}

    /// <summary>
    /// This class is to lookup language with given localID
    /// </summary>
    class GeoLanguageLookup : GeoDataFeedBase
    {
        private Dictionary<int, string> localeLookup;
        internal Dictionary<int, string> LocaleLookup
        {
            get { return localeLookup; }
        }

        /// <summary>
        /// LocaleId->Language lookup. 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetLanguage(int input)
        {
            string val = null;
            if (localeLookup.TryGetValue(input, out val))
                return val;
            else
                return localeLookup[MSCOMGeoConfig.DefaultLocale];
        }
        public GeoLanguageLookup()
        {
            dbCapacitySP = Helper.GetConfigureString("GeoLanguageLookupCountGetForSocket", "GeoLanguageLookupCountGetForSocket");
            dbDataSP = Helper.GetConfigureString("GeoLanguageLookupSelectForSocket", "GeoLanguageLookupSelectForSocket");
        }


        protected override void PopulateDataFromFile(BinaryReader r)
        {
            int listCount = r.ReadInt32();
            localeLookup = new Dictionary<int, string>(listCount);
            Int32 id;
            string val1;

            for (int i = 0; i < listCount; i++)
            {
                id = r.ReadInt32();
                val1 = r.ReadString();
                localeLookup.Add(id, val1);
            }
        }

        protected override void WriteDataToFile(BinaryWriter w)
        {
            w.Write(localeLookup.Count);

            foreach (KeyValuePair<int, string> entry in localeLookup)
            {
                w.Write(entry.Key);
                w.Write(entry.Value);                
            }
        }

        public override void Clear()
        {
            if (this.localeLookup != null)
            {
                this.localeLookup.Clear();
            }
        }
    }

	#endregion

	#region
	class GeoTimeZoneData
	{
		public string StZoneOffSet;
		public string DstZoneOffSet;
		public string DstStartTime;
		public string DstEndTime;
		public string StName;
		public string DstName;
	}

	class GeoTimeZone : GeoDataFeedBase
	{		
		Dictionary<string, GeoTimeZoneData> geoTimeZoneLookup;
		private static string keyLocaleSplitor = ":";		
		public GeoTimeZone()
		{
			dbDataSP = Helper.GetConfigureString("GeoTimeZoneSelectForSocket", "GeoTimeZoneSelectForSocket");
			dbCapacitySP = Helper.GetConfigureString("GeoTimeZoneCountGetForSocket", "GeoTimeZoneCountGetForSocket");
		}

		public GeoTimeZoneData GetByKey(int id, string localeID)
		{
			GeoTimeZoneData val;
			if (geoTimeZoneLookup.TryGetValue(id + keyLocaleSplitor + localeID, out val))
				return val;
		    return null;
		}
        
		protected override void PopulateDataFromFile(BinaryReader r)
		{
			int listCount = r.ReadInt32();
			geoTimeZoneLookup = new Dictionary<string, GeoTimeZoneData>(listCount);
			string key;
			
			for (int i = 0; i < listCount; i++)
			{
				GeoTimeZoneData data = new GeoTimeZoneData();
				key = r.ReadString();
				data.DstEndTime = r.ReadString();
				data.DstName = r.ReadString();
				data.DstStartTime = r.ReadString();
				data.DstZoneOffSet = r.ReadString();
				data.StName = r.ReadString();
				data.StZoneOffSet = r.ReadString();
				geoTimeZoneLookup.Add(key, data);
			}
		}

		override protected void WriteDataToFile(BinaryWriter w)
		{
			w.Write(geoTimeZoneLookup.Count);
			foreach (KeyValuePair<string, GeoTimeZoneData> entry in geoTimeZoneLookup)
			{
				w.Write(entry.Key);
				w.Write(entry.Value.DstEndTime);
				w.Write(entry.Value.DstName);
				w.Write(entry.Value.DstStartTime);
				w.Write(entry.Value.DstZoneOffSet);
				w.Write(entry.Value.StName);
				w.Write(entry.Value.StZoneOffSet);
			}
		}

        public override void Clear()
        {
            if (this.geoTimeZoneLookup != null)
            {
                this.geoTimeZoneLookup.Clear();
            }
        }
	}
	#endregion
}
