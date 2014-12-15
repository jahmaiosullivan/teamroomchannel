using System;
using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class IntroViewModel
    {
        public virtual long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public IEnumerable<string> Images { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserViewModel CreatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public UserViewModel LastUpdatedBy { get; set; }
    }
}