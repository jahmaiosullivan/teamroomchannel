using System;
using System.Collections.Generic;
using System.Globalization;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoCommon
{
    public enum GeoEntityLevel : int
    {
		None = 0,
        CountryRegion = 1,
        StateProvince = 2,
        City = 3
    }
    /// <summary>
    /// parent child entity relationship will be a tree structure
    /// </summary>
    public class GeoEntityNode
    {
        private int id;
        private bool supportPostalCode;
        private GeoEntityLevel level = GeoEntityLevel.City; // 3: city, 2: state, 1: country
        private string code;
        private bool isEU;
        private string[] localizedNames;
        /// <summary>
        /// this is the localized names for a geoentity. the order is defined in GeoInfoLookup.GetLocalizedNamePositionByLocalId()
        /// </summary> 
        public string[] LocalizedNames
        {
            get { return localizedNames; }
            set { localizedNames = value; }
        }

        /// <summary>Only applicable when level = 1 (country node)</summary>
        public bool IsEU
        {
            get { return isEU; }
        }
        /// <summary>
        /// GeoEntity code
        /// </summary>
        public string Code
        {
            get { return code; }
        }
        /// <summary>
        /// 3: city, 2: state, 1: country
        /// </summary> 
        public GeoEntityLevel Level
        {
            get {return level;}
        }
        /// <summary>
        /// Support Postalcode or not
        /// </summary>
        public bool SupportPostalCode
        {
            get { return supportPostalCode; }
        } 
        /// <summary>
        /// Mappoint ID
        /// </summary>
        public int Id
        {
            get { return id; }
        }
        internal List<GeoEntityNode> Children;

        private GeoEntityNode(){}

        internal GeoEntityNode(int mappointId, int geolevel, int supportPostalCodeId, bool iseu, string geoCode)
        {
            id = mappointId;
            if (supportPostalCodeId == 2)
                supportPostalCode = true;
            level = (GeoEntityLevel)geolevel;
            isEU = iseu;
            code = geoCode;
        }

        internal void AddChild(GeoEntityNode node)
        {
            if (Children == null)
                Children = new List<GeoEntityNode>(128);
            Children.Add(node);
        }
    }
    /// <summary>
    /// This class is used to persist information to feed the request of GetCountryRegionList.
    /// </summary>
    class GeoEntityRelation : GeoDataFeedBase
    {
        protected Dictionary<int, GeoEntityNode> geoCRandSPLookup; //This 
        protected GeoEntityNode Root = new GeoEntityNode(-1, 0, 0, false, null);
        public GeoEntityRelation()
        {
            dbCapacitySP = Helper.GetConfigureString("GeoEntityRelationCountGetForSocket", "GeoEntityRelationCountGetForSocket"); ;
            dbDataSP = Helper.GetConfigureString("GeoEntityRelationGetForSocket", "GeoEntityRelationGetForSocket");
        }

     
        protected override void PopulateDataFromFile(System.IO.BinaryReader r)
        {
            int count = r.ReadInt32();
            geoCRandSPLookup = new Dictionary<int, GeoEntityNode>(count);

            Int32 id;
            Int32 pid;
            Int16 level;
            Int32 expandable;
            bool supportPostalCode;
            bool iseu;
            string code;
            GeoEntityNode node;
            GeoEntityNode parentNode;
            while (true)
            {
                try
                {
                    pid = r.ReadInt32();
                    id = r.ReadInt32();
                    level = r.ReadInt16();
                    supportPostalCode = r.ReadBoolean();
                    iseu = r.ReadBoolean();
                    code = r.ReadString();

                    expandable = r.ReadInt32();

                    node = new GeoEntityNode(id, level, (supportPostalCode?2:0), iseu, code);

                    if (pid == -1)
                        Root.AddChild(node);
                    else
                    {
                        if (geoCRandSPLookup.TryGetValue(pid, out parentNode))
                            parentNode.AddChild(node);
                        else
                            throw new ApplicationException("Read file wrong in GeoEntityRelation!");
                    }
                    if (expandable == 1)
                        geoCRandSPLookup.Add(id, node);
                }
                catch (System.IO.EndOfStreamException)
                {
                    break;
                }
            }
        }

        protected override void WriteDataToFile(System.IO.BinaryWriter w)
        {
            w.Write(geoCRandSPLookup.Count);
            writeChildren(w, Root);
        }

        void writeChildren(System.IO.BinaryWriter w, GeoEntityNode parent)
        {
            if (parent != null && parent.Children != null)
            {
                for (int i = 0; i < parent.Children.Count; i++)
                {
                    w.Write(parent.Id);
                    w.Write(parent.Children[i].Id);
                    w.Write((short)parent.Children[i].Level);
                    w.Write(parent.Children[i].SupportPostalCode);
                    w.Write(parent.Children[i].IsEU);
                    w.Write(parent.Children[i].Code);

                    if (parent.Children[i].Children != null)
                    {
                        w.Write(1);
                        writeChildren(w, parent.Children[i]);
                    }
                    else
                        w.Write(0);
                }
            }
        }

        public override void Clear()
        {
            if (this.geoCRandSPLookup != null)
            {
                this.geoCRandSPLookup.Clear();
            }
        }

        /// <summary>
        /// Given a mappoint id, get the entityNode from the tree
        /// this is done by dictionary lookup.
        /// </summary>
        /// <param name="id">id less than 0: means root node</param>
        /// <returns></returns>
        public GeoEntityNode GetEntityNodeByID(int id)
        {
            if (id < 0)
                return Root;
            else{
                GeoEntityNode node;
                geoCRandSPLookup.TryGetValue(id, out node);
                return node;
            }
        }

        #region this is the code for fast GeoEntity localized name access
        Dictionary<int, int> localePositionLookup;

        internal int GetLocalizedNamePositionByLocalId(int localeId)
        {
            int result;
            if (localePositionLookup.TryGetValue(localeId, out result))
                return result;
            else
                return MSCOMGeoConfig.DefaultLocale;
        }

        /// <summary>
        /// This function can be called only when all the 4 instances in GeoInfoLookup are available
        /// </summary>
        /// <param name="langLookup"></param>
        /// <param name="cr"></param>
        /// <param name="sp"></param>
        /// <param name="c"></param>
        internal void PopulateLocalizedNameForNodes(GeoLanguageLookup langLookup, GeoCountryRegion cr, GeoStateProvince sp, GeoCity c)
        {
            localePositionLookup = new Dictionary<int, int>(langLookup.LocaleLookup.Count);
            int currentPos = 0;
            foreach (int key in langLookup.LocaleLookup.Keys)
            {
                localePositionLookup.Add(key, currentPos);
                currentPos++;
            }

            PopulateNodeChildren(Root, cr, sp, c);            
        }

        void PopulateNodeChildren(GeoEntityNode node, GeoCountryRegion cr, GeoStateProvince sp, GeoCity c)
        {
            if (node == null || node.Children == null)
                return;
            for (int i = 0; i < node.Children.Count; i++)
            {
                PopulateSingleNode(node.Children[i], cr, sp, c);
                PopulateNodeChildren(node.Children[i], cr, sp, c);   
            }
        }
        void PopulateSingleNode(GeoEntityNode node, GeoCountryRegion cr, GeoStateProvince sp, GeoCity c)
        {
            node.LocalizedNames = new string[localePositionLookup.Count];
            foreach (KeyValuePair<int, int> kv in localePositionLookup)
            {
                node.LocalizedNames[kv.Value] = GeoInfoLookup.GetGeoEntityNameByIDLocaleLevel(node.Id, node.Level, kv.Key.ToString(CultureInfo.InvariantCulture), cr, sp, c);
            }
        }
        #endregion
    }
}
