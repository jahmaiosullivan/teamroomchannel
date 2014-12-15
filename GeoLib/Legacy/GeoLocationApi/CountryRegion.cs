using System.Collections.Generic;
using Microsoft.MSCOM.Geo.GeoLocationAPI;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoLocationApi
{
	/// <summary>
	/// CountryRegion class for GeoEntity
	/// </summary>
    public class CountryRegion : GeoEntity
    {
        #region "Fields and Properties"
        private bool isEU;
        private bool supportPostalCode;
        private int languageId;

		/// <summary>
		/// Language Id
		/// </summary>
        public int LanguageId
        {
            get { return languageId; }
            internal set { languageId = value; }
        }

		/// <summary>
		/// If this country is belong to EU or not
		/// </summary>
        public bool IsEU
        {
            get { return isEU; }
            internal set { isEU = value; }
        }

		/// <summary>
		/// Whether this CountryRegion support PostalCode or not
		/// </summary>
        public bool SupportPostalCode
        {
            get { return supportPostalCode; }
            set { supportPostalCode = value; }
        }

        #endregion


        #region Constructor
        internal CountryRegion(int id, string code)
            : base(id, code)
        {
            this.languageId = MSCOMGeoConfig.DefaultLocale; // default to English
        }

        internal CountryRegion(int id, string code, int languageId)
            : base(id, code)
        {
            this.languageId = languageId;
        }
        #endregion

       
    }
}
