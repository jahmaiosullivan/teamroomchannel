using System.Collections.Generic;

namespace HobbyClue.Web.Models
{
    public class HomePageQuery
    {
        public HomePageQuery()
        {
            Page = 1;
        }
        public string SearchTerm { get; set; }
        public string Forum { get; set; }
        public string Filter { get; set; }
        public string Sort { get; set; }
        public int? Page { get; set; }
        public string ThreadId { get; set; }
        public bool BrandIgnore { get; set; }

        public List<string> Filters { get; set; }
        public string SearchTermId { get; set; }
        public bool ExactMatchEnabled { get; set; }
        public string PageTitle { get; set; }
        public string PageHeading { get; set; }
    }
}