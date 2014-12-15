using System;
using System.Collections.Generic;
using System.Text;
using MSCOMGeoSystem.Common;
namespace Microsoft.MSCOM.Geo.GeoLocationAPI
{
	/// <summary>
	/// City class of GeoEntity
	/// </summary>
    public class City : GeoEntity
    {
        #region "Fields and Properties"

        private int parentId;
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
		/// Parent Mappoint ID. Normally it is a StateProvince Id or a CountryRegion ID
		/// </summary>
        public int ParentId
        {
            get { return parentId; }
            internal set { parentId = value; }
        }

        #endregion

        #region "Constructors"
        internal City(int id, string code, int parentId)
            : base(id, code)
        {
            this.languageId = MSCOMGeoConfig.DefaultLocale; // default to English
            ParentId = parentId;
        }

        internal City(int id, string code, int parentId, int languageId)
            : base(id, code)
        {
            this.languageId = languageId;
            ParentId = parentId;
        }

        #endregion

        #region "Member Function"
       
        #endregion
    }
}
