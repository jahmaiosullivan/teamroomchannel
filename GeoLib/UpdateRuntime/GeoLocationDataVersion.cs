using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace Microsoft.IT.Geo.UpdateRuntime
{
    //=========================================================================
    //  Class: GeoLocationDataVersion
    //
    /// <summary>
    /// Version information for the Geo Location data
    /// </summary>
    //=========================================================================

    [DataContract]
    [Serializable]
    public sealed class GeoLocationDataVersion :
        IComparable<GeoLocationDataVersion>
    {
        //=====================================================================
        //  Method: GeoLocationDataVersion
        //
        /// <summary>
        /// Default constructor
        /// </summary>
        //=====================================================================

        private GeoLocationDataVersion()
        {
        }

        //=====================================================================
        //  Method: VersionHash
        //
        /// <summary>
        /// The MD5 hash of the geo data computed
        /// </summary>  
        /// 
        //=====================================================================

        [DataMember(EmitDefaultValue=false)]
        public byte[] VersionHash { get; private set; }

        //=====================================================================
        //  Method: CreateConfiguration
        //
        /// <summary>
        /// This method returns the default configuration
        /// </summary>  
        //=====================================================================

        public static GeoLocationDataVersion ComputeVersion(
            Stream locationData
            )
        {
            GeoLocationDataVersion versionInfo = null;
            MD5 hashAlgorithm = null;
            if (locationData != null)
            {
                try
                {
                    versionInfo = new GeoLocationDataVersion();

                    hashAlgorithm = MD5.Create();

                    versionInfo.VersionHash = hashAlgorithm.ComputeHash(
                        locationData
                        );
                }
                finally
                {
                    if (hashAlgorithm != null)
                    {
                        hashAlgorithm.Dispose();
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(
                    "locationData",
                    "Cannot compute version iinformation for null location data."
                    );
            }

            return versionInfo;
        }

        //=====================================================================
        //  Method: ToString
        ///
        /// <summary>
        /// This method returns the profile ID as formatted string
        /// </summary>
        //=====================================================================

        public override string ToString()
        {
            Debug.Assert(this.VersionHash != null);

            StringBuilder versionBuilder = new StringBuilder();

            for (int index = this.VersionHash.Length - 1; index >= 0; --index)
            {
                versionBuilder.AppendFormat(
                    "{0:X2}",
                    this.VersionHash[index]
                    );
            }

            return versionBuilder.ToString();
        }

        //=====================================================================
        //  Method: ==
        //
        /// <summary>
        /// This method true if the values are equal
        /// </summary>
        //=====================================================================

        public static bool operator ==(GeoLocationDataVersion version1, GeoLocationDataVersion version2)
        {
            int difference = GeoLocationDataVersion.Compare(version1, version2);

            return difference == 0;
        }

        //=====================================================================
        //  Method: !=
        //
        /// <summary>
        /// This method true if the values are equal
        /// </summary>
        //=====================================================================

        public static bool operator !=(GeoLocationDataVersion version1, GeoLocationDataVersion version2)
        {
            return !(version1 == version2);
        }


        //=====================================================================
        //  Method: Equals
        //
        /// <summary>
        /// This method true if the values are equal
        /// </summary>
        //=====================================================================

        public override bool Equals(
            object obj
            )
        {
            GeoLocationDataVersion version2 = obj as GeoLocationDataVersion;

            int result = GeoLocationDataVersion.Compare(this, version2);

            return ((result == 0) ? true : false);
        }

        //=====================================================================
        //  Method: GetHashCode
        //
        /// <summary>
        /// This method overrides GetHashCode
        /// </summary>
        //=====================================================================

        public override int GetHashCode()
        {
            Debug.Assert(this.VersionHash != null);

            int hashCode = 0;

            for (int index = this.VersionHash.Length - 1; index >= 0; --index)
            {
                byte value = this.VersionHash[index];

                hashCode += value;
                hashCode <<= 2;
            }

            return hashCode;
        }

        //=====================================================================
        //  Method: CompareTo
        //
        /// <summary>
        /// This method implements the IComparable.CompareTo method
        /// </summary>
        //=====================================================================

        public int CompareTo(
            GeoLocationDataVersion other
            )
        {
            return GeoLocationDataVersion.Compare(
                this,
                other
                );
        }

        //=====================================================================
        //  Method: Compare
        //
        /// <summary>
        /// This method compares two versions. If equal the return value is 0.
        /// If version1 < version2 then result is positive, otherwise it's
        /// negative.
        /// </summary>
        //=====================================================================

        public static int Compare(
            GeoLocationDataVersion version1,
            GeoLocationDataVersion version2
            )
        {
            object object1 = (object)(version1);
            object object2 = (object)(version2);

            if (object1 == null && object2 == null)
            {
                return 0;
            }

            if (object1 == null && object2 != null)
            {
                return -1;
            }

            if (object1 != null && object2 == null)
            {
                return 1;
            }

            Debug.Assert(object1 != null);
            Debug.Assert(object2 != null);

            if (version1.VersionHash == null && version2.VersionHash != null)
            {
                return -1;
            }

            if (version1.VersionHash != null && version2.VersionHash == null)
            {
                return 1;
            }

            Debug.Assert(version1.VersionHash.Length == version2.VersionHash.Length);

            int index = version1.VersionHash.Length - 1;

            Debug.Assert(index >= 0);

            int diff = 0;

            while ((index > 0) && (diff == 0))
            {
                int value1 = version1.VersionHash[index];
                int value2 = version2.VersionHash[index];

                diff = (value1 - value2);

                index--;
            }

            return diff;
        }
    }
}
