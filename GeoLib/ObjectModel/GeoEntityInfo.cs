using System;
using System.Runtime.Serialization;

namespace Microsoft.IT.Geo.ObjectModel
{
    /// <summary>
    /// Abstract base class for all geo Entity classes
    /// </summary>
    [DataContract]
    public class GeoEntityInfo : IComparable
    {

        #region "Fields and Properties"
        private string code;
        private string displayName;
        private string displayLanguage;
        private int id;

        /// <summary>
        /// Id of this Entity
        /// </summary>
         [DataMember(EmitDefaultValue = false)]
        public int Id
        {
            get { return id; }
            internal set { id = value; }
        }


        /// <summary>
        /// Code for this Entity
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public string Code
        {
            get { return code; }
            internal set { code = value; }
        }
        /// <summary>
        /// Display name for this entity
        /// </summary>
         [DataMember(EmitDefaultValue = false)]
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
        [DataMember(EmitDefaultValue = false)]
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
        internal GeoEntityInfo(int id, string code)
        {
            this.Id = id;
            this.Code = code;
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
            GeoEntityInfo geoEntity = obj as GeoEntityInfo;

            if (geoEntity != null)
                return string.Compare(DisplayName, geoEntity.DisplayName, StringComparison.OrdinalIgnoreCase);
            else
                throw new ArgumentException("Parameter in GeoEntity.CompareTo is invalid");
        }

        #endregion
    }
}
