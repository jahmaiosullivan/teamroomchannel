using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.MSCOM.Geo.GeoLocationAPI
{
	/// <summary>
	/// Abstract base class for all Entity classes
	/// </summary>
    public abstract class Entity
    {
        #region "Fields and Properties"
        private int id;
		/// <summary>
		/// Id of this Entity
		/// </summary>
        public int Id
        {
            get { return id; }
            internal set { id = value; }
        }

        #endregion
        #region "Constructors"

        internal Entity(int id)
        {
            this.Id = id;
        }
        #endregion
    }
}
