using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoCommon
{
	/// <summary>
	/// This class represents all the information for a Geographic location.
	/// </summary>
    public class GeoInfoCommon
    {
        #region "Fields and Properties"
        private int mapPointId;
		private int mapPointIdParent;
		private string countryRegion;
        private string countryRegionCode;
        private string stateProvince;
        private string stateProvinceCode;
		private string city;
		private double? longitude = null;
		private double? latitude = null;
		private bool isEU;
		private string timeZone;
		private string dsttimeZone;
		private double? standardTimeZoneOffset = null;
		private double? dstTimeZoneOffset = null;
		private DateTime? dstStartTimeUniversal = null;
		private DateTime? dstEndTimeUniversal = null;
		private string postalCodeInfo;

		/// <summary>
		/// This is the ID from map point
		/// </summary>
		public int MapPointId
		{
			get { return mapPointId; }
			set { mapPointId = value; }
		}
		/// <summary>
		/// Parent Mappoint ID. for example, if the current MapPointId is a city id, then parent ID could be a stateregion ID.
		/// </summary>
		public int MapPointIdParent
		{
			get { return mapPointIdParent; }
			set { mapPointIdParent = value; }
		}

		/// <summary>
		/// Display name of a country region
		/// </summary>
		public string CountryRegionDisplayName
		{
			get { return countryRegion; }
			set { countryRegion = value; }
		}
		/// <summary>
		/// The code for country region
		/// </summary>
		public string CountryRegionCode
		{
			get { return countryRegionCode; }
			set { countryRegionCode = value; }
		}

		/// <summary>
		/// The display name for StateProvince
		/// </summary>
		public string StateProvinceDisplayName
		{
			get { return stateProvince; }
			set { stateProvince = value; }
		}
		/// <summary>
		/// This code for StateProvince
		/// </summary>
		public string StateProvinceCode
        {
            get { return stateProvinceCode; }
            set { stateProvinceCode = value; }
        }

		/// <summary>
		/// City display name
		/// </summary>
		public string CityDisplayName
		{
			get { return city; }
			set { city = value; }
		}

		/// <summary>
		/// Longitude of this Geo Location
		/// </summary>
		public double? Longitude
		{
			get { return longitude; }
			set { longitude = value; }
		}
		/// <summary>
		/// Latitude of this Geo location
		/// </summary>
		public double? Latitude
		{
			get { return latitude; }
			set { latitude = value; }
		}

		/// <summary>
		/// If this location is belong to EU
		/// </summary>
        public bool IsEuropeanUnion
        {
            get { return isEU; }
            set { isEU = value; }
        }

		/// <summary>
		/// Timezone display name
		/// </summary>
		public string TimeZoneDisplayName
		{
			get { return timeZone; }
			set { timeZone = value; }
		}

		/// <summary>
		/// Dst (Date saving) timezone display name
		/// </summary>
		public string DstTimeZoneDisplayName
		{
			get { return dsttimeZone; }
			set { dsttimeZone = value; }
		}

		/// <summary>
		/// Standard timezone display name
		/// </summary>
		public double? StandardTimeZoneOffset
		{
			get { return standardTimeZoneOffset; }
			set { standardTimeZoneOffset = value; }
		}
		/// <summary>
		/// Dst (date saving) timezone offset
		/// </summary>
		public double? DstTimeZoneOffset
		{
			get { return dstTimeZoneOffset; }
			set { dstTimeZoneOffset = value; }
		}
		/// <summary>
		/// Dst start time for this location
		/// </summary>
		public DateTime? DstStartTimeUniversal
		{
			get { return dstStartTimeUniversal; }
			set { dstStartTimeUniversal = value; }
		}
		/// <summary>
		/// Dst end time for this location
		/// </summary>
		public DateTime? DstEndTimeUniversal
		{
			get { return dstEndTimeUniversal; }
			set { dstEndTimeUniversal = value; }
		}
		/// <summary>
		/// This is the postalcode string for this location.
		/// </summary>
		public string PostalCodeInfo
		{
			get { return postalCodeInfo; }
			set { postalCodeInfo = value; }
		}

		private ReadOnlyCollection<string> postalCodes;		
		/// <summary>
		/// If the postalcodeInfo is sth like 98052+98053-98055, we will return 98052,98053,98054,98055 in a collection. 
		/// This property might have negative performance impact if the postalcodeInfo has a long list of codes. So please only use it as necessary.
		/// Note: 
		/// 1. '+' is the main seperator. the whole string is seperated by + into parts
		/// 2. For each part, if it does not have exactly one '-', or it has other non numeric characters, just return as it is
		/// 3. For a part with exactly one '-' and all numerical characters, we return the whole range. But if the range start value > end value, we just return as it is (it should not happen according to our db)
		/// 4. Here '+' is configured as postalCodeSeperatorCharacter, '-' is configured as postalCodeRangeSeperatorCharacter		/// 
		/// </summary>
		public ReadOnlyCollection<string> PostalCodes
		{
			get
			{
				if ( postalCodes == null )
				{
					postalCodes = ParsePostalCodeList(this);
				}
				return postalCodes;
			}
		}

		/// <summary>
		/// If a postalCode list is sth like 98052+98053-98055
		/// we should return 98052+98053+98054+98055
		/// Note: 
		/// 1. '+' is the main separator. the whole string is separated by + into parts
		/// 2. For each part, if it does not have exactly one '-', or it has other non numeric characters, just return as it is
		/// 3. For a part with exactly one '-' and all numerical characters, we return the whole range. But if the range start value > end value, we just return as it is (it should not happen according to our db)
		/// 4. Here '+' is configured as PostalCodeSeperatorCharacter, '-' is configured as PostalCodeRangeSeperatorCharacter
		/// </summary>
		/// <param name="geoInfo">geoinfo object</param>		
		/// <returns>ReadOnlyCollection of strings. If nothing there, an empty collection will be returned. It will not return null</returns>
		public static ReadOnlyCollection<string> ParsePostalCodeList(GeoInfoCommon geoInfo)
		{
			char postalCodeSeparator = MSCOMGeoConfig.PostalCodeSeparatorCharacter[0];
			char postalCodeRangeSeparator = MSCOMGeoConfig.PostalCodeRangeSeperatorCharacter[0];

			if (geoInfo == null || string.IsNullOrEmpty(geoInfo.PostalCodeInfo))
				return new ReadOnlyCollection<string>(new List<string>());

			string[] parts = geoInfo.PostalCodeInfo.Split(postalCodeSeparator);
			List<string> resultList = new List<string>(parts.Length);
			for (int i = 0; i < parts.Length; i++)
			{
				if (parts[i].IndexOf(postalCodeRangeSeparator) < 0)
					resultList.Add(parts[i]);
				else
				{
					string[] postalcodeRange = parts[i].Split(postalCodeRangeSeparator);
					if (postalcodeRange.Length != 2 || postalcodeRange[0].Length != postalcodeRange[1].Length)
						resultList.Add(parts[i]); // unknown '-' usage, or length is different for the range, just return as it is.
					else
					{
						int start;
						int end;
						// if not numeric, return the whole part
						if ((!Int32.TryParse(postalcodeRange[0], out start)) || (!Int32.TryParse(postalcodeRange[1], out end)))
						{
							resultList.Add(parts[i]);
							continue;
						}
						if (start > end || start < 0 || end < 0)
						{
							resultList.Add(parts[i]);
							continue;
						}

						resultList.Add(postalcodeRange[0]);
						int length = postalcodeRange[0].Length;
						for (int postalcode = start + 1; postalcode < end; postalcode++)
						{
                            string temp = postalcode.ToString(CultureInfo.InvariantCulture);
							if (temp.Length < length)
								resultList.Add(postalcodeRange[0].Substring(0, length - temp.Length) + temp); //if postalcode is 00099-00200, then need to make sure we return 5 digits
							else
								resultList.Add(temp);
						}
						if (end > start)
							resultList.Add(postalcodeRange[1]);
					}
				}
			}
			return new ReadOnlyCollection<string>(resultList);
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		/// <remarks>
		/// Returns all fields of the Geo Info in one string. Used only for unit test purposes and not for public usage.
		/// </remarks>
		public override string ToString()
		{
			StringBuilder geoInfoString = new StringBuilder();
            geoInfoString.AppendFormat("MapPointId: {0}{1}", MapPointId.ToString(CultureInfo.InvariantCulture), Environment.NewLine);
			geoInfoString.AppendFormat("MapPointIdParent: {0}{1}", MapPointIdParent.ToString(CultureInfo.InvariantCulture), Environment.NewLine);
			geoInfoString.AppendFormat("CountryRegionDisplayName: {0}{1}", (CountryRegionDisplayName== null)?"NULL":CountryRegionDisplayName.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("CountryRegionCode: {0}{1}", (CountryRegionCode == null) ? "NULL" : CountryRegionCode.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("StateProvinceDisplayName: {0}{1}", (StateProvinceDisplayName == null) ? "NULL" : StateProvinceDisplayName.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("StateProvinceCode: {0}{1}", (StateProvinceCode == null) ? "NULL" : StateProvinceCode.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("CityDisplayName: {0}{1}", (CityDisplayName == null) ? "NULL" : CityDisplayName.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("Longitude: {0}{1}", (Longitude == null) ? "NULL" : Longitude.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("Latitude: {0}{1}", (Latitude == null) ? "NULL" : Latitude.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("IsEuropeanUnion: {0}{1}", IsEuropeanUnion.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("TimeZoneDisplayName: {0}{1}", (TimeZoneDisplayName == null) ? "NULL" : TimeZoneDisplayName.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("DSTTimeZoneDisplayName: {0}{1}", (DstTimeZoneDisplayName == null) ? "NULL" : DstTimeZoneDisplayName.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("StandardTimeZoneOffset: {0}{1}", (StandardTimeZoneOffset == null) ? "NULL" : StandardTimeZoneOffset.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("DSTTimeZoneOffset: {0}{1}", (DstTimeZoneOffset == null) ? "NULL" : DstTimeZoneOffset.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("DSTStartTimeUniversal: {0}{1}", (DstStartTimeUniversal == null) ? "NULL" : DstStartTimeUniversal.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("DSTEndTimeUniversal: {0}{1}", (DstEndTimeUniversal == null) ? "NULL" : DstEndTimeUniversal.ToString(), Environment.NewLine);
			geoInfoString.AppendFormat("PostalCodeInfo: {0}{1}", (PostalCodeInfo == null) ? "NULL" : PostalCodeInfo.ToString(), Environment.NewLine);

			geoInfoString.Append("PostalCodes:");
			if (PostalCodes.Count == 0)
			{
				geoInfoString.Append(" NULL");
			}
			foreach (string postalCode in PostalCodes)
			{
				geoInfoString.AppendFormat(" {0}", postalCode);
			}
			geoInfoString.AppendFormat("{0}", Environment.NewLine);

			return geoInfoString.ToString();
		}

        #endregion

    }

}
