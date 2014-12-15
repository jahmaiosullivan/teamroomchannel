using System.Collections.Generic;
using System.Globalization;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoCommon
{
    /// <summary>
    /// This class will provide (CountryCode, Zipcode) -> CityID lookup                
    /// </summary>
    class GeoPostalCodeLookup : GeoDataFeedBase
    {
        Dictionary<string, bool> partialCountry;
        Dictionary<string, int> postalCodeLookup;
        public GeoPostalCodeLookup()
        {
            dbCapacitySP = Helper.GetConfigureString("GeoPostalCodeLookupCountGetForSocket", "GeoPostalCodeLookupCountGetForSocket");;
            dbDataSP = Helper.GetConfigureString("GeoPostalCodeLookupGetForSocket", "GeoPostalCodeLookupGetForSocket");
        }
        protected override void PopulateDataFromFile(System.IO.BinaryReader r)
        {
            int length = r.ReadInt32();
            partialCountry = new Dictionary<string, bool>(length);
            for (int i = 0; i < length; i++)
                partialCountry.Add(r.ReadString(), true);
            length = r.ReadInt32();
            postalCodeLookup = new Dictionary<string, int>(length);
            string key;
            int val;
            for (int i = 0; i < length; i++)
            {
                key = r.ReadString();
                val = r.ReadInt32();
                postalCodeLookup.Add(key, val);
            }
        }

        protected override void WriteDataToFile(System.IO.BinaryWriter w)
        {
            w.Write(partialCountry.Count);
            foreach(string cty in partialCountry.Keys)
            {
                w.Write(cty);
            }
            w.Write(postalCodeLookup.Count);
            foreach(KeyValuePair<string, int> entry in postalCodeLookup)
            {
                w.Write(entry.Key);
                w.Write(entry.Value);
            }
        }

        public int LookupPostalCode(string postalCode, string countryCode)
        {
            postalCode = postalCode.ToLower(CultureInfo.InvariantCulture);
            countryCode = countryCode.ToLower(CultureInfo.InvariantCulture);
            if (partialCountry.ContainsKey(countryCode))
            {
                if (postalCode.Length > 3)
                    postalCode = postalCode.Substring(0, 3);
            }
            int val = -1;
            if (postalCodeLookup.TryGetValue(countryCode + ":" + postalCode, out val))
                return val;
            else
                return -1;
        }

        public override void Clear()
        {
            if (this.postalCodeLookup != null)
            {
                this.postalCodeLookup.Clear();
            }
        }
    }
}
