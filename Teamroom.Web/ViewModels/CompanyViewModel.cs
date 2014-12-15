using System;
using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class CompanyViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Logo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserViewModel CreatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public UserViewModel LastUpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> MyConversations { get; set; }
        public IEnumerable<string> CommunityConversations { get; set; }
        public IEnumerable<UserViewModel> Team { get; set; }
        public IntroViewModel Intro { get; set; }
    }
}