using System.Collections.Generic;
using MSCOMGeoSystem.Common;
namespace Microsoft.MSCOM.Geo.GeoLocationAPI
{
	/// <summary>
	/// StateProvince entity
	/// </summary>
    public class StateProvince : GeoEntity
    {
        #region "Fields and Properties"
        private int parentId;
        private int languageId;
       
		/// <summary>
		/// LanguageId of this entity
		/// </summary>
        public int LanguageId
        {
            get { return languageId; }
            internal set { languageId = value; }
        }
		/// <summary>
		/// Parent ID (CountryRegion ID)
		/// </summary>
        public int ParentId
        {
            get { return parentId; }
            internal set { parentId = value; }
        }

        


        #endregion

        #region "Constructor"

        internal StateProvince(int id, string code, int parentId)
            : base(id, code)
        {            
            ParentId = parentId;
            this.languageId = MSCOMGeoConfig.DefaultLocale; // default to English
        }


        internal StateProvince(int id, string code, int parentId, int languageId)
            : base(id, code)
        {
            ParentId = parentId;
            this.languageId = languageId;
        }

        #endregion

    }
}
