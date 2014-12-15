using System;

namespace Microsoft.MSCOM.Geo.GeoLocationAPI
{
	/// <summary>
	/// Abstract base class for all geo Entity classes
	/// </summary>
    public abstract class GeoEntity : Entity, IComparable
    {
        #region "Fields and Properties"
        private string code;
        private string displayName;
        private string displayLanguage;

		/// <summary>
		/// Code for this Entity
		/// </summary>
        public string Code
        {
            get { return code;}
            internal set { code = value; }
        }
		/// <summary>
		/// Display name for this entity
		/// </summary>
        public string DisplayName
        {
            get
            {
                if (displayLanguage == null)
                    displayLanguage = "en";
                return displayName;
            }
            internal set { displayName = value; }
        }

		/// <summary>
		/// Display Language of this entity
		/// </summary>
        public string DisplayLanguage
        {
            get { return displayLanguage; }
            internal set { displayLanguage = value; }
        }
        #endregion

        #region "Constructor"
		/// <summary>
		/// Constructor a new Geo Entity
		/// </summary>
		/// <param name="id">Entity ID (from mappoint)</param>
		/// <param name="code">Entity Code</param>
        protected GeoEntity(int id, string code)
            : base(id)
        {
            Code = code;
        }
        #endregion

        #region IComparable Members
        /// <summary>
        /// This provides the means of compare with another GeoEntity object's DisplayName. It is case insensitive comparison.
        /// </summary>
        /// <param name="obj">object must be GeoEntity's sub-classes</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
			GeoEntity geoEntity = obj as GeoEntity;

            if (geoEntity != null)
                return string.Compare(DisplayName, geoEntity.DisplayName, StringComparison.OrdinalIgnoreCase);
            else
                throw new ArgumentException("Parameter in GeoEntity.CompareTo is invalid");
        }

        #endregion
    }
}
