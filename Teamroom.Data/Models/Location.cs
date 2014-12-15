using System.ComponentModel.DataAnnotations;

namespace HobbyClue.Data.Models
{
    public class Location : ModelBase
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
