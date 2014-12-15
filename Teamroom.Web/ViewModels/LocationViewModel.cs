using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HobbyClue.Data.Models;
using HobbyClue.Web.Configuration.TypeConverters;

namespace HobbyClue.Web.ViewModels
{
    [TypeConverter(typeof(LocationTypeConverter))]
    public class LocationViewModel : BaseDapperModel
    {
        public string GoogleId { get; set; }

        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }

        public string Country { get; set; }

        public string GoogleGeometryJson { get; set; }

        public string PhoneNumber { get; set; }
    }
}
