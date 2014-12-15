using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HobbyClue.Web.ViewModels
{
    public class GoogleLocation
    {
        public AddressComponent[] address_components { get; set; }
        public string formatted_address { get; set; }
        public string formatted_phone_number { get; set; }

        public Geometry geometry { get; set; }

        public string icon { get; set; }

        public string id { get; set; }

        public string international_phone_number { get; set; }

        public string name { get; set; }

        public Photo[] photos { get; set; }

        public string[] types { get; set; }

        public string website { get; set; }

        public string tz { get; set; }
    }

    public class Geometry
    {
        public GeometryLocation location { get; set; }
    }

    public class GeometryLocation
    {
        public string d { get; set; }
        public string e { get; set; }
    }

    public class Photo
    {
        public int height { get; set; }
        public int width { get; set; }
        public string[] html_attributions { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public string short_name { get; set; }
        public string[] types { get; set; }
    }
}