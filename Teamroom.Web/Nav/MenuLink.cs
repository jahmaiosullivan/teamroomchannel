using System.Collections.Generic;

namespace HobbyClue.Web.Nav
{
    public class MenuLink
    {
        public MenuLink(LinkType type, bool isSelected)
        {
            Id = (int) type;
            IsSelected = isSelected;
            Children = new List<MenuLink>();
        }

        public int Id { get; private set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public bool IsSelected { get; set; }
        public IList<MenuLink> Children { get; set; }
    }
}
