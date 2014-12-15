using System;
using System.Collections.Generic;
using Microsoft.IT.Geo.Legacy.GeoCommon;
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

    public sealed class SubnetRangeDataComparer : IComparer<SubnetRangeData>
    {
        #region IComparer<SubnetRangeData> Members

        //============================================================================
        //	Method: Compare
        //
        /// <summary>
        ///	IComparer<>.Compare implementation
        /// </summary>
        //============================================================================

        int IComparer<SubnetRangeData>.Compare(SubnetRangeData x, SubnetRangeData y)
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
    //	Class: SubnetRangeData
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
        Pack = 1
        )]
    public struct SubnetRangeData
    {
        public SubnetRangeData(byte[] subnetBegin, byte[] subnetEnd, int mask)
        {
            this.SubnetBegin = subnetBegin;
            this.SubnetEnd = subnetEnd;
            this.Mask = mask;
        }

        public byte[] SubnetBegin;

        public byte[] SubnetEnd;

        public Int32 Mask;

        
    }

    /// <summary>The data object to store (SubnetBegin, GeoId)</summary>		
    class SubnetRangeLookup : GeoDataFeedBase
    {
        private static SubnetRangeDataComparer comparer = new SubnetRangeDataComparer();
        private List<SubnetRangeData> subnetRangeList;

        private int addressLength = 4;

        /// <summary>Constructor using Mask</summary>		
        public SubnetRangeLookup(bool isIPv4)
        {
            base.dbCapacitySP = null;

            base.dbDataSP = "GetGeoReservedInfo";

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
            this.subnetRangeList = new List<SubnetRangeData>(listCount);

            byte[] subnetBeginInfo;

            byte[] subnetEndInfo;
            
            Int32 mask;

            SubnetRangeData rangeItem;

            for (int i = 0; i < listCount; i++)
            {
                subnetBeginInfo = r.ReadBytes(
                    this.addressLength
                    );

                subnetEndInfo = r.ReadBytes(
                    this.addressLength
                    );

                mask = r.ReadInt32();

                rangeItem = new SubnetRangeData(
                    subnetBeginInfo,
                    subnetEndInfo,
                    mask
                    );

                this.subnetRangeList.Add(rangeItem);
            }
        }

        /// <summary>Save data to file</summary>		
        override protected void WriteDataToFile(BinaryWriter w)
        {
            w.Write(this.subnetRangeList.Count);
            for (int i = 0; i < this.subnetRangeList.Count; i++)
            {
                w.Write(this.subnetRangeList[i].SubnetBegin);
                w.Write(this.subnetRangeList[i].SubnetEnd);
                w.Write(this.subnetRangeList[i].Mask);
            }
        }

        public override void Clear()
        {
            if (this.subnetRangeList != null)
            {
                this.subnetRangeList.Clear();
            }
        }

        /// <summary> Look up IP using binary search, return its geoID or -1 if not found</summary>		
        public bool IsReserved(IPAddress ipAddress)
        {
            bool isReserved = false;

            if (ipAddress == null)
            {
                isReserved = false;
            }
            else if (this.subnetRangeList.Count > 0)
            {
                isReserved = this.Search(
                    ipAddress.GetAddressBytes(),
                    0,
                    this.subnetRangeList.Count - 1
                    );
            }

            return isReserved;
        }

        private bool Search(
            byte[] ipAddressBytes,
            int startIndex,
            int endIndex
            )
        {
            IComparer<SubnetRangeData> subnetComparer = new SubnetRangeDataComparer();

            int midPoint;
            byte[] ipMid;
            int compareResult = 0;

            bool isReserved = false;

            if (this.subnetRangeList.Count > 0)
            {
                while (startIndex < endIndex - 1)
                {
                    midPoint = (startIndex + endIndex) >> 1;
                    ipMid = this.subnetRangeList[midPoint].SubnetBegin;

                    compareResult = SubnetRangeLookup.Compare(
                        ipAddressBytes,
                        ipMid
                        );

                    if (compareResult == 0)
                    {
                        return true;
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

                //
                //  Compare with the last one, as we can be in either
                //  of the two ranges, so the SubnetBegin would be less
                //  then or equal both ranges to the IP looked for
                //
                //  Modified 3/16 as the Reserved ranges are not "merged"
                //  meaning that two ranges, can be adjacent, like 10.0.0.0/8
                //  and 10.10.10.10/32, where the first range would contain the second
                //  but, because they are not merged, we would fail to find a 10.100.*.* address
                //  as being reserved
                //  The actual fix would be to merge the Reserved ranges too, same like
                //  the other ranges are, but for now, we would verify in both ranges
                //  found last, but there might still be a very small chance, if the address
                //  ranges are many, not our case, and very dispersed
                //

                int compareResultStart = SubnetRangeLookup.Compare(
                    this.subnetRangeList[endIndex].SubnetBegin,
                    ipAddressBytes
                    );

                int compareResultEnd = SubnetRangeLookup.Compare(
                    this.subnetRangeList[endIndex].SubnetEnd,
                    ipAddressBytes
                    );

                if (compareResultStart <= 0
                    && compareResultEnd >= 0
                    )
                {
                    isReserved = true;
                }
                else
                {
                    //
                    //  Verify the start range too
                    //

                    compareResultStart = SubnetRangeLookup.Compare(
                        this.subnetRangeList[startIndex].SubnetBegin,
                        ipAddressBytes
                        );

                    compareResultEnd = SubnetRangeLookup.Compare(
                        this.subnetRangeList[startIndex].SubnetEnd,
                        ipAddressBytes
                        );

                    if (compareResultStart <= 0
                        && compareResultEnd >= 0
                        )
                    {
                        isReserved = true;
                    }
                }

                //if (compareResultStart <= 0)
                //{
                //    //  the ip address has a chance of being in the range
                //    //  for the end index

                //    compareResultEnd = SubnetRangeLookup.Compare(
                //        this.subnetRangeList[endIndex].SubnetEnd,
                //        ipAddressBytes
                //        );
                //}
                //else
                //{
                //    compareResultStart = SubnetRangeLookup.Compare(
                //        this.subnetRangeList[startIndex].SubnetBegin,
                //        ipAddressBytes
                //        );

                //    if (compareResultStart <= 0)
                //    {
                //        compareResultEnd = SubnetRangeLookup.Compare(
                //            this.subnetRangeList[startIndex].SubnetEnd,
                //            ipAddressBytes
                //            );
                //    }
                //}

                //if (compareResultStart <= 0 && compareResultEnd >= 0)
                //{
                //    isReserved = true;
                //}
            }

            return isReserved;
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
