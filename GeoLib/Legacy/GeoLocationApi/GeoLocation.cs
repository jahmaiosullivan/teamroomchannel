using System.Collections.ObjectModel;
using System.Web;
using Microsoft.IT.Geo.Legacy.GeoCommon;
using Microsoft.MSCOM.Geo.GeoLocationAPI;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoLocationApi
{
    #region Enumerations

#if DEBUG
    public enum ReturnFromWhere
    {
        None = 0,
        Socket = 1,
        Database = 2,
        HighSpeed = 3
    }
#endif

    #endregion
	/// <summary>
	/// Provides the primary entry-points for interacting with the GeoSegmentation API.
    /// Returns GeoInfo objects from different lookup methods, and provides
    /// the GetCountryRegionList method for building lists of Countries, States, 
    /// and Cities
	/// </summary>
	public static class GeoLocation
	{
		#region Helper Methods
        
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
		internal static string GetHttpRequestLanguage(HttpRequest httpRequest)
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

		/// <summary>
		/// Parses the postal code list inside a GeoInfo object into a collection of strings.
		/// </summary>
		/// <param name="geoInfo">geoinfo object</param>
		/// <returns>
		/// ReadOnlyCollection of strings. If nothing there, an empty collection will be returned. It will not return null
		/// </returns>
		/// <remarks>
		/// If a postalCode list is sth like 98052+98053-98055
		/// we should return 98052+98053+98054+98055
		/// Note:
		/// 1. '+' is the main separator. the whole string is separated by + into parts
		/// 2. For each part, if it does not have exactly one '-', or it has other non numeric characters, just return as it is
		/// 3. For a part with exactly one '-' and all numerical characters, we return the whole range. But if the range start value &gt; end value, we just return as it is (it should not happen according to our db)
		/// 4. Here '+' is configured as PostalCodeSeperatorCharacter, '-' is configured as PostalCodeRangeSeperatorCharacter
		/// </remarks>
		internal static ReadOnlyCollection<string> ParsePostalCodeList(GeoInfo geoInfo)
		{
			return GeoInfoCommon.ParsePostalCodeList(geoInfo.GeoInfoCommon);
		}


		#endregion


	}
}
		