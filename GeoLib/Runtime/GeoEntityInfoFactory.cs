using System.Collections.Generic;
using Microsoft.IT.Geo.ObjectModel;
using Microsoft.MSCOM.Geo.GeoLocationAPI;

namespace Microsoft.IT.Geo.Runtime
{
    //=====================================================================
    //  Class: GeoEntityInfoFactory
    //
    /// <summary>
    /// Functionality for creating List of Entity Info
    /// </summary>  
    //=====================================================================

    public static class GeoEntityInfoFactory
    {
        //=====================================================================
        //  Method: GeoEntityInfoFactory
        //

        //=====================================================================
        //  Method: Create
        //
        /// <summary>
        /// Create a GeoLocationInfo instance from a GeoInfoCommon instance
        /// </summary>  
        //=====================================================================

        internal static List<GeoEntityInfo> Create(
            List<GeoEntity> list
            )
        {
            var resultList = new List<GeoEntityInfo>();
            
            if (list != null)
            {
                foreach (GeoEntity node in list)
                {
                    GeoEntityInfo result = new GeoEntityInfo(node.Id, node.Code);
                    result.DisplayLanguage = node.DisplayLanguage;
                    result.DisplayName = node.DisplayName;
                    resultList.Add(result);  
                }
            }

            return resultList;
        }
    }
}
