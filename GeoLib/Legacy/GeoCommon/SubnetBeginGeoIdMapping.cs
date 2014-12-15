using System;
using System.Collections.Generic;
using Microsoft.IT.Geo.Legacy.GeoCommon;
using MSCOMGeoSystem.Common;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace Microsoft.IT.Geo
{
    //============================================================================
    //	Class: SubnetBeginAndGeoIdComparer
    //
    /// <summary>
    ///	Comparer for SubnetBeginAndGeoID
    /// </summary>
    //============================================================================

    public sealed class SubnetBeginGeoIdComparer : IComparer<SubnetBeginGeoId>
    {
        #region IComparer<SubnetBeginAndGeoID> Members

        //============================================================================
        //	Method: Compare
        //
        /// <summary>
        ///	IComparer<>.Compare implementation
        /// </summary>
        //============================================================================

        int IComparer<SubnetBeginGeoId>.Compare(SubnetBeginGeoId x, SubnetBeginGeoId y)
        {
            int result = 0;

            if (x.SubnetBegin != null
                && y.SubnetBegin != null)
            {
                //
                //  Both have values, so we compare them byte by byte
                //

                if (x.SubnetBegin.Length == y.SubnetBegin.Length)
                {
                    for (int i = 0; i < x.SubnetBegin.Length; i++)
                    {
                        result = x.SubnetBegin[i] - y.SubnetBegin[i];

                        if (result != 0)
                        {
                            break;
                        }
                    }
                }
                else if (x.SubnetBegin.Length > y.SubnetBegin.Length)
                {
                    result = 1;
                }
                else
                {
                    result = -1;
                }

            }
            else if (x.SubnetBegin == null)
            {
                result = -1;
            }
            else
            {
                result = 1;
            }

            return result;
        }

        #endregion
    }

    //============================================================================
    //	Class: SubnetBeginGeoId
    //
    /// <summary>
    ///	The struct to keep subnetbegin and GeoID mapping. It is used for binary
    ///	search
    /// It is purposefully not using class. 
    /// Using struct, it takes only 3 seconds to load data from file to memory,
    /// and consumes 170M memory after loading. 
    /// But if we use class, it takes 6 seconds to load, and occupies 330M memory
    /// after load.
    /// </summary>
    //============================================================================

    [StructLayout(
        LayoutKind.Sequential,
        Pack=1
        )]
    public struct SubnetBeginGeoId
    {
        public SubnetBeginGeoId(byte[] subnetBegin, int geoId)
        {
            this.SubnetBegin = subnetBegin;
            this.GeoId = geoId;
        }

        public byte[] SubnetBegin;

        public Int32 GeoId;
    }

    /// <summary>The data object to store (SubnetBegin, GeoId)</summary>		
    class SubnetBeginGeoIdMapping : GeoDataFeedBase
    {
        private List<SubnetBeginGeoId> subnetBeginGeoIdList;

        private int addressLength = 4;

        

        /// <summary>Constructor using Mask</summary>		
        public SubnetBeginGeoIdMapping(bool isIPv4)
        {
            dbCapacitySP = Helper.GetConfigureString(
                "GeoSubnetBeginCountGetForSocketV2", 
                "GeoSubnetBeginCountGetForSocketV2"
                );

            dbDataSP = Helper.GetConfigureString(
                "GeoSubnetBeginAndGeoIndexIDSelectForSocketV2", 
                "GeoSubnetBeginAndGeoIndexIDSelectForSocketV2"
                );

            if (isIPv4 == false)
            {
                this.addressLength = 16;
            }
            else
            {
                this.addressLength = 4;
            }
        }


        /// <summary> Load data from file </summary>		
        protected override void PopulateDataFromFile(BinaryReader r)
        {
            int listCount = r.ReadInt32();
            this.subnetBeginGeoIdList = new List<SubnetBeginGeoId>(listCount);

            byte[] subnetBeginInfo;
            Int32 val2;
            for (int i = 0; i < listCount; i++)
            {
                subnetBeginInfo = r.ReadBytes(
                    this.addressLength
                    );
                val2 = r.ReadInt32();
                this.subnetBeginGeoIdList.Add(
                    new SubnetBeginGeoId(subnetBeginInfo, val2)
                    );
            }
        }

        /// <summary>Save data to file</summary>		
        override protected void WriteDataToFile(BinaryWriter w)
        {
            w.Write(this.subnetBeginGeoIdList.Count);
            for (int i = 0; i < this.subnetBeginGeoIdList.Count; i++)
            {
                w.Write(this.subnetBeginGeoIdList[i].SubnetBegin);
                w.Write(this.subnetBeginGeoIdList[i].GeoId);
            }
        }

        public override void Clear()
        {
            if (this.subnetBeginGeoIdList != null)
            {
                this.subnetBeginGeoIdList.Clear();
            }
        }

        /// <summary> Look up IP using binary search, return its geoID or -1 if not found</summary>		
        public int Lookup(IPAddress ipAddress)
        {
            if (ipAddress == null)
            {
                return -1;
            }

            return this.Search(
                ipAddress.GetAddressBytes(),
                0, 
                this.subnetBeginGeoIdList.Count - 1
                );
        }

        private int Search(
            byte[] ipAddressBytes, 
            int startIndex, 
            int endIndex
            )
        {
            IComparer<SubnetBeginGeoId> subnetComparer = new SubnetBeginGeoIdComparer();

            int midPoint;
            byte[] ipMid;
            int compareResult = 0;

            while (startIndex < endIndex - 1)
            {
                midPoint = (startIndex + endIndex) >> 1;
                ipMid = this.subnetBeginGeoIdList[midPoint].SubnetBegin;

                compareResult = SubnetBeginGeoIdMapping.Compare(
                    ipAddressBytes,
                    ipMid
                    );

                if (compareResult == 0)
                {
                    return this.subnetBeginGeoIdList[midPoint].GeoId;
                }
                else if (compareResult > 0)
                {
                    startIndex = midPoint;
                }
                else
                {
                    endIndex = midPoint - 1;
                }
            }

            compareResult = SubnetBeginGeoIdMapping.Compare(
                this.subnetBeginGeoIdList[endIndex].SubnetBegin,
                ipAddressBytes
                );

            if (compareResult > 0)
            {
                return this.subnetBeginGeoIdList[startIndex].GeoId;
            }
            else
            {
                return this.subnetBeginGeoIdList[endIndex].GeoId;
            }
        }


        private static int Compare(byte[] x, byte[] y)
        {
            int result = 0;

            if (x != null
                && y != null)
            {
                //
                //  Both have values, so we compare them byte by byte
                //

                if (x.Length == y.Length)
                {
                    for (int i = 0; i < x.Length; i++)
                    {
                        result = x[i] - y[i];

                        if (result != 0)
                        {
                            break;
                        }
                    }
                }
                else if (x.Length > y.Length)
                {
                    result = 1;
                }
                else
                {
                    result = -1;
                }

            }
            else if (x == null)
            {
                result = -1;
            }
            else
            {
                result = 1;
            }

            return result;
        }
    }
}
