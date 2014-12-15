using System.Collections.Generic;

namespace HobbyClue.Web.Areas.Admin.Models
{
    public class SidebarMenuItem
    {
        public SidebarMenuItem()
        {
            Children = new List<SidebarMenuItem>();
        }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsFirst { get; set; }
        public bool IsOpen { get; set; }
        public bool IsSelected { get; set; }
        public string Icon { get; set; }
        public bool IsHeading { get; set; }
        public string BadgeText { get; set; }
        public IEnumerable<SidebarMenuItem> Children { get; set; }
    }
}