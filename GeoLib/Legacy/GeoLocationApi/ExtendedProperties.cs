using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.IT.Geo.Legacy.GeoCommon;
using MSCOMGeoSystem.Common;

namespace Microsoft.IT.Geo.Legacy.GeoLocationApi
{
    internal class ExtProperty
    {
        private string _name;
        private string _defaultValue;
        Dictionary<string , string> _values;
        Dictionary<string , string> _regions;
  
        internal ExtProperty(string propName)
        {
            _name = propName;
            _values = new Dictionary<string, string>(5);
            _regions = new Dictionary<string, string>(10);
           
        }

        internal string Name
        {
            get
            {
                return _name;
            }
             set
            {
                _name = value;
                
            }
        }

        internal string DefaultValue
        {
            get
            {
                return _defaultValue;
            }
              set
            {
                _defaultValue = value;
            }
        }

        internal Dictionary<string , string> Values
        {
            get
            {
              return _values;
            }
              set
            {
                _values = value;
            }
        }

        internal Dictionary<string , string> Regions
        {
            get
            {
                return _regions;
            }
              set
            {
                _regions = value;
            }
        }

    }

  /// <summary>
  /// ExtendedProperties class allows adopters to associate set of custom properties with geographic locations. Access to these properties is provided via geoInfo class thus giving a seamless experience.
  /// </summary>

    internal static class ExtendedProperties
    {
        private static Hashtable customProperties ;
        private static XmlReaderSettings xmlReaderSettings ;
        private static DateTime lastLoad;
        private static FileSystemWatcher extendedPropWatcher;
         

        internal static Hashtable Properties
        {
          get
          {
            return customProperties;
          }
        }

        static ExtendedProperties()
        {
          
            customProperties = new Hashtable(3);
            MSCOMGeoConfig.ExtPropFileNameChangeHandler += new EventHandler(HandleFileNameChange);

            xmlReaderSettings = new XmlReaderSettings();
              
            xmlReaderSettings.Schemas.Add(null, System.IO.Path.Combine( MSCOMGeoConfig.DBDataPath  , MSCOMGeoConfig.ExtendedPropertySchemaFile));
            xmlReaderSettings.ValidationType = ValidationType.Schema;
            xmlReaderSettings.IgnoreComments = true;
            xmlReaderSettings.IgnoreWhitespace = true;

          
            // use file monitor to monitor the extendedproperty file
            if (!(string.IsNullOrEmpty(MSCOMGeoConfig.ExtendedPropertyConfigFile)))
            {
    
                ExtendedProperties.LoadExtendedProperties(null, null);
    
                if ( extendedPropWatcher != null)
                    extendedPropWatcher = null;
                  
                extendedPropWatcher = new FileSystemWatcher();
                extendedPropWatcher.Path = Path.GetDirectoryName(MSCOMGeoConfig.ExtendedPropertyConfigFile);
                extendedPropWatcher.Filter = Path.GetFileName(MSCOMGeoConfig.ExtendedPropertyConfigFile);
                extendedPropWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                extendedPropWatcher.Changed += new FileSystemEventHandler(LoadExtendedProperties);
                extendedPropWatcher.Created += new FileSystemEventHandler(LoadExtendedProperties);
                extendedPropWatcher.EnableRaisingEvents = true;
            }

        }


        internal static void HandleFileNameChange(object source, EventArgs eArgs)
        {
            if (!(string.IsNullOrEmpty(MSCOMGeoConfig.ExtendedPropertyConfigFile)))
            {
                LoadExtendedProperties(null, null);

                if (extendedPropWatcher != null)
                    extendedPropWatcher = null;

                extendedPropWatcher = new FileSystemWatcher();
                extendedPropWatcher.Path = Path.GetDirectoryName(MSCOMGeoConfig.ExtendedPropertyConfigFile);
                extendedPropWatcher.Filter = Path.GetFileName(MSCOMGeoConfig.ExtendedPropertyConfigFile);
                extendedPropWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName;
                extendedPropWatcher.Changed += new FileSystemEventHandler(LoadExtendedProperties);
                extendedPropWatcher.Created += new FileSystemEventHandler(LoadExtendedProperties);
                extendedPropWatcher.EnableRaisingEvents = true;
            }
        }

        internal static void LoadExtendedProperties(object source, 
                                              System.IO.FileSystemEventArgs e)
        {
            //load File

          //Ignore changes in last 500 milli seconds. FileSystemWatcher sends multiple events for one change. Time is set to 500 milli seconds so we don't lose any events in normal changes.
          if (lastLoad != null && 
              ((TimeSpan)DateTime.Now.Subtract(lastLoad)).TotalMilliseconds < 500){
            return;
          } else {
            lastLoad = DateTime.Now;
          }

          Hashtable tempProperties = new Hashtable();

          System.IO.StreamReader streamReader = null;

          try
          {

              streamReader = new System.IO.StreamReader(GeoDataFeedBase.SafeOpenFileForRead(MSCOMGeoConfig.ExtendedPropertyConfigFile));


              using (XmlReader xmlReader = XmlReader.Create(streamReader, xmlReaderSettings))
              {
                  ExtProperty current = null;

                  #region commented_old_code_using_reader
                  /*
              while (xmlReader.Read())
              {

                while (xmlReader.ReadToFollowing("property"))
                {
                  // for each property
                  current = new ExtProperty(xmlReader.GetAttribute("name"));

                  if (xmlReader.ReadToFollowing("values"))
                  {
                    current.DefaultValue = xmlReader.GetAttribute("defaultValue");

                    if (xmlReader.ReadToDescendant("value"))
                    {
                      do
                      {
                        current.Values[xmlReader.GetAttribute("id")] =
                                    xmlReader.GetAttribute("data");
                      } while (xmlReader.ReadToNextSibling("value"));

                    }
                  }//values

                  if (xmlReader.ReadToFollowing("regions"))
                  {
                    if (xmlReader.ReadToDescendant("region")) // found region
                    {
                      do
                      {
                        current.Regions[xmlReader.GetAttribute("name")] =
                                  xmlReader.GetAttribute("valueId");
                      } while (xmlReader.ReadToNextSibling("region"));
                    }

                  }
                  tempProperties[current.Name.ToLower()] = current;
                }
                break;
              }//Read
               * 
               * */
                  #endregion

                  #region new_code_with_dom
                  XmlDocument doc = new XmlDocument();
                  doc.Load(xmlReader);

                  XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                  nsmgr.AddNamespace("ext", "urn:extendedproperties-schema");


                  XmlNodeList propList = doc.GetElementsByTagName("property");
                  foreach (XmlNode property in propList)
                  {
                      current = new ExtProperty(property.Attributes["name"].Value);

                      //values

                      XmlNode valuesElem = property.SelectSingleNode("ext:values", nsmgr);

                      // XmlNode valuesElem = property.FirstChild;

                      current.DefaultValue = valuesElem.Attributes["defaultValue"].Value;


                      if (valuesElem.HasChildNodes)
                      {
                          XmlNodeList valueElems = valuesElem.ChildNodes;
                          foreach (XmlNode value in valueElems)
                          {
                              current.Values[value.Attributes["id"].Value] = value.Attributes["data"].Value;

                          }
                          valueElems = null;
                      }
                      valuesElem = null;

                      XmlNode regionsElem = property.SelectSingleNode("ext:regions", nsmgr);
                      if (regionsElem != null)
                      {
                          XmlNodeList regionElems = regionsElem.ChildNodes;
                          foreach (XmlNode region in regionElems)
                          {
                              current.Regions[region.Attributes["name"].Value] = region.Attributes["valueId"].Value;
                          }
                          regionElems = null;
                      }

                      regionsElem = null;
                      tempProperties[current.Name.ToLower(CultureInfo.InvariantCulture)] = current;


                  }//foreach property
                  propList = null;

                  #endregion

              } //using 


              if (tempProperties.Count > 0)
              {
                  lock (customProperties.SyncRoot)
                  {
                      customProperties = tempProperties;
                  }
                 //EventLogWriter.WriteEntry(EventType.GeoCommon, "ExtendedProperties:Successfully Loaded Extended Properties Config File", System.Diagnostics.EventLogEntryType.Information);

              }
          }
          finally
          {
              tempProperties.Clear();
              tempProperties = null;
              if (streamReader != null)
              {
                  streamReader.Close();
              }
          }
        }//end method

        
        
    }//end class
}
