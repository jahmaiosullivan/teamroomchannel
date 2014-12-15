using System;
using System.Collections.Generic;
using System.IO;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoCommon
{
	/// <summary>
	/// The base class, which defines the methods for each data object
	/// </summary>
    public abstract class GeoDataFeedBase
    {
		#region protected members (should be initialized by subclass's constructor)		
		/// <summary>The sp to get the capacity to initialize the data structure</summary>
		protected string dbCapacitySP;
		/// <summary>The sp to retrieve the data</summary>
		protected string dbDataSP;
		#endregion

		
		#region protected/abstract methods which must be implemented by subclass
		
		/// <summary>Load data from file to data structure</summary>
		abstract protected void PopulateDataFromFile(BinaryReader reader);

		/// <summary>Save data structure into file</summary>
		abstract protected void WriteDataToFile(BinaryWriter writer);

        /// <summary>Save data structure into file</summary>
        public abstract void Clear();

		#endregion

		#region public methods which should not be overriden
	
		/// <summary>Load data from file</summary>		
		public void LoadFromFile(BinaryReader reader)
		{
			PopulateDataFromFile(reader);
		}

		public void SaveToFile(BinaryWriter writer)
		{
			WriteDataToFile(writer);			
		}
		#endregion 

		#region protected methods which should not be overriden
		/// <summary>
		/// Open file safely for read access 
		/// When we open a file for read and something is writing this file, we should wait until that is done.
		/// </summary>
		/// <param name="filename">filename for read</param>		
		/// <returns>File stream for read</returns>
		public static FileStream SafeOpenFileForRead(string fileName)
		{
            if (!File.Exists(fileName))
            {
                //EventLogWriter.WriteEntry(EventType.GeoCommon, "GeoDataFeedBase:Unable to find: " + fileName);
                return null;
            }
			FileStream document = null;
			int numOfTries = 0;

			while (document == null)
			{
				try
				{
					document = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
				}
				catch (Exception e)
				{
					if (e is IOException && numOfTries < MSCOMGeoConfig.SafeOpenFileTimeout)
					{
						numOfTries++;
						System.Threading.Thread.Sleep(1000);
					}
					else
					{
						//EventLogWriter.WriteException(EventType.GeoCommon, "GeoDataFeedBase:Unable to load: " + fileName, e);
						return null;
					}
				}
			}
			return document;
		}
		#endregion
	}

    /// <summary>Comparer for SubnetBeginAndGeoID</summary>		
    public sealed class SubnetBeginAndGeoIdComparer : IComparer<SubnetBeginAndGeoId>
    {
        #region IComparer<SubnetBeginAndGeoID> Members

        int IComparer<SubnetBeginAndGeoId>.Compare(SubnetBeginAndGeoId x, SubnetBeginAndGeoId y)
        {
            if (x.SubnetBegin != y.SubnetBegin)
            {
                byte xbyte;
                byte ybyte;
                xbyte = (byte)(x.SubnetBegin >> 24 & 0xFF);
                ybyte = (byte)(y.SubnetBegin >> 24 & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;

                xbyte = (byte)(x.SubnetBegin >> 16 & 0xFF);
                ybyte = (byte)(y.SubnetBegin >> 16 & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;

                xbyte = (byte)(x.SubnetBegin >> 8 & 0xFF);
                ybyte = (byte)(y.SubnetBegin >> 8 & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;


                xbyte = (byte)(x.SubnetBegin & 0xFF);
                ybyte = (byte)(y.SubnetBegin & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;
            }
            return 0;
        }

        #endregion
    }
	
	/// <summary>
	/// The struct to keep subnetbegin and GeoID mapping. It is used for binary search
	/// It is purposefully not using class. 
	/// Using struct, it takes only 3 seconds to load data from file to memory, and consumes 170M memory after loading. 
	/// But if we use class, it takes 6 seconds to load, and occupies 330M memory after load.
	/// </summary>
	public struct SubnetBeginAndGeoId
	{
		public SubnetBeginAndGeoId(int subnetBeginValue, int geoIdValue)
		{
            SubnetBegin = subnetBeginValue;
            GeoId = geoIdValue;
		}
		public Int32 SubnetBegin;
		public Int32 GeoId;		
	}

	/// <summary>The data object to store (SubnetBegin, GeoId)</summary>		
    class SubnetBeginAndGeoIdList : GeoDataFeedBase
    {
		private List<SubnetBeginAndGeoId> subnetBeginAndGeoIDList;
        
		/// <summary>Constructor using Mask</summary>		
        public SubnetBeginAndGeoIdList()
        {            
			dbCapacitySP = Helper.GetConfigureString("GeoSubnetBeginCountGetForSocketV2", "GeoSubnetBeginCountGetForSocketV2");
			dbDataSP = Helper.GetConfigureString("GeoSubnetBeginAndGeoIndexIDSelectForSocketV2", "GeoSubnetBeginAndGeoIndexIDSelectForSocketV2");			
        }
        
	
		/// <summary> Load data from file </summary>		
		protected override void PopulateDataFromFile(BinaryReader r)
        {			
			int listCount = r.ReadInt32();			
			subnetBeginAndGeoIDList = new List<SubnetBeginAndGeoId>(listCount);			
			Int32 val1;
			Int32 val2;
			for (int i = 0; i < listCount; i++)
			{
				val1 = r.ReadInt32();
				val2 = r.ReadInt32();
				subnetBeginAndGeoIDList.Add(new SubnetBeginAndGeoId(val1, val2));
			}			
        }

		/// <summary>Save data to file</summary>		
        override protected void WriteDataToFile(BinaryWriter w)
        {			
			w.Write(subnetBeginAndGeoIDList.Count);			
			for (int i = 0; i < subnetBeginAndGeoIDList.Count; i++)
			{
				w.Write(subnetBeginAndGeoIDList[i].SubnetBegin);				
				w.Write(subnetBeginAndGeoIDList[i].GeoId);
			}			
        }

        public override void Clear()
        {
            if (this.subnetBeginAndGeoIDList != null)
            {
                this.subnetBeginAndGeoIDList.Clear();
            }
        }

		/// <summary> Look up IP using binary search, return its geoID or -1 if not found</summary>		
		public int Lookup(int ip)
		{
			if (ip == 0)
				return -1;            
            return Search(ip, 0, subnetBeginAndGeoIDList.Count-1);			
		}

		/*
		private bool IsMatch(int ip1, int ip2)
		{
			return (ip1 & subNetMasks) == (ip2 & subNetMasks);
		}*/

        /// <summary>
        /// Note: in V1, we just return x-y which is incorrect. when x is negative, it is actually a big number, so we should always compare bytes
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
		private static int Compare(int x, int y)
		{
            if (x != y)
            {
                byte xbyte;
                byte ybyte;
                xbyte = (byte)(x >> 24 & 0xFF);
                ybyte = (byte)(y >> 24 & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;

                xbyte = (byte)(x >> 16 & 0xFF);
                ybyte = (byte)(y >> 16 & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;

                xbyte = (byte)(x >> 8 & 0xFF);
                ybyte = (byte)(y >> 8 & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;


                xbyte = (byte)(x & 0xFF);
                ybyte = (byte)(y & 0xFF);
                if (xbyte != ybyte)
                    return xbyte - ybyte;
            }
            return 0;
		}

		private int Search(int ip, int startIndex, int endIndex)
		{
            int midPoint;
            int ipMid;
            while (startIndex < endIndex - 1)
            {
                midPoint = (startIndex + endIndex) >> 1;
                ipMid = subnetBeginAndGeoIDList[midPoint].SubnetBegin;
                if (ipMid == ip) //TODO: if remove this line
                    return subnetBeginAndGeoIDList[midPoint].GeoId;
                else if (Compare(ip, ipMid) > 0)
                    startIndex = midPoint;
                else
                {
                    endIndex = midPoint - 1;
                }
            }
            if (Compare(subnetBeginAndGeoIDList[endIndex].SubnetBegin, ip) > 0)
                return subnetBeginAndGeoIDList[startIndex].GeoId;
            else
                return subnetBeginAndGeoIDList[endIndex].GeoId;
		}		
	}
}
