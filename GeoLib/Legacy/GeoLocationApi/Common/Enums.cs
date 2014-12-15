namespace Microsoft.IT.Geo.Legacy.GeoLocationApi.Common
{
    /// <summary>
    /// It has 3 values:
    ///		None: does not need postalcode in result
    ///		PostalCodeExact: return all postalcodes in that city
    ///		PostalCodeExactPlus35Miles: return all postalcodes in that city, plus all postalcodes in a predefined neighbor area. 
    /// </summary>
    public enum GeoPostalCodeRangeMode
    {
        /// <summary>
        /// Do not need postalcode in result
        /// </summary>
        None = 0,
        /// <summary>
        /// Return all postalcodes in that city
        /// </summary>
        PostalCodeExact = 1,
        /// <summary>
        /// Return all postalcodes in that city, plus all postalcodes within 35 miles from that city
        /// </summary>
        PostalCodeExactPlus35Miles = 2
    }
}
