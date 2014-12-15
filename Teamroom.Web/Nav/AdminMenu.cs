using System.Collections.Generic;

namespace HobbyClue.Web.Nav
{
    public class AdminMenu : List<MenuLink>
    {
        public AdminMenu(LinkType selected)
        {
            Add(CreateMenuLink(LinkType.AdminDashboard, selected));

            var manageEventsLink = CreateMenuLink(LinkType.AdminEvents, selected);
            manageEventsLink.Children.Add(CreateMenuLink(LinkType.AdminCreateEvent, selected, "Add New Event"));
            Add(manageEventsLink);

            Add(CreateMenuLink(LinkType.AdminUsers, selected));
            Add(CreateMenuLink(LinkType.AdminCategories, selected));
        }

        private static MenuLink CreateMenuLink(LinkType type, LinkType selected, string title = null)
        {
            var menuName = type.ToString().Substring(5);
            menuName = menuName.ToCharArray()[0].ToString().ToUpper() + menuName.ToLower().Substring(1);

            return new MenuLink(type, (type == selected)) { Title = title ?? "Manage " + menuName, Url = "/admin/" + menuName.ToLower() };
        }
    }
}
