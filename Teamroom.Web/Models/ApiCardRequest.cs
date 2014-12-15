using System;
using System.Collections.Generic;

namespace HobbyClue.Web.Models
{
    public class ApiCardRequest
    {
        public ApiCardRequest()
        {
            Tags = new List<string>();
        }

        public string CityName { get; set; }
        public string Region { get; set; }
        public long FromId { get; set; }
        public int Page { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}