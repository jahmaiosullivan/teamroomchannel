using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class AdminSidebarViewModelItem
    {
        public string Title { get; set; }
        public string IconClass { get; set; }
        public string Url { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }

        public bool IsActive { get; set; }

        public IEnumerable<AdminSidebarViewModelItem> Children { get; set; }
    }
}