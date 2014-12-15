using System.Collections.Generic;
using HobbyClue.Web.Areas.Admin.Models;
using HobbyClue.Web.Helpers;

namespace HobbyClue.Web.Areas.Admin.ModelBuilders
{
    public interface ISidebarModelBuilder
    {
        IEnumerable<SidebarMenuItem> BuildAdmin();
    }

    public class SidebarModelBuilder : ISidebarModelBuilder
    {
        private readonly MvcUrlHelper mvcUrlHelper;
        public SidebarModelBuilder(MvcUrlHelper mvcUrlHelper)
        {
            this.mvcUrlHelper = mvcUrlHelper;
        }

        public IEnumerable<SidebarMenuItem> BuildAdmin()
        {
            var menuItems = new List<SidebarMenuItem>();
            var dashBoardMenuItem = Create("Dashboard", "home", true, "/admin");
            var reviewsMenuItem = Create("Reviews", "home", true, "/admin/reviews");
            var messagesMenuItem = Create("Messages", "home", true, "/admin/messages");

            var fansMenuItem = Create("Fans",isHeading:true);
            fansMenuItem.Children = new List<SidebarMenuItem>
                                        {
                                            CreateJavaScriptMenuItem("All fans", "settings",badge:"26"),
                                            CreateJavaScriptMenuItem("Top fans", "settings",badge:"1")
                                        };


            var eventsMenuItem = CreateHeading("Events");
            eventsMenuItem.Children = new List<SidebarMenuItem>
                                        {
                                            CreateJavaScriptMenuItem("New", "settings"),
                                            Create("Upcoming Events", "basket", url: "/products/reviews", badge:"19"),
                                            Create("Past Events", "basket", url: "/products/reviews", badge:"19")
                                        };

            var reportsMenuItem = Create("Reports", isHeading: true);
            reportsMenuItem.Children = new List<SidebarMenuItem>
                                        {
                                            CreateJavaScriptMenuItem("Average Attendance", "settings")
                                        };

            menuItems.Add(dashBoardMenuItem);
            menuItems.Add(messagesMenuItem);
            menuItems.Add(reviewsMenuItem);
            menuItems.Add(fansMenuItem);
            menuItems.Add(eventsMenuItem);
            return menuItems;
        }

        private SidebarMenuItem CreateJavaScriptMenuItem(string title, string icon = null, bool isFirst = false, bool isHeading = false, string badge = null, bool isOpened = false)
        {
            return Create(title, icon, isFirst, "javascript:;", isHeading, badge, isOpened);
        }
        private SidebarMenuItem CreateHeading(string title)
        {
            return Create(title, isHeading: true);
        }
        private SidebarMenuItem Create(string title, string icon = null, bool isFirst = false, string url = null, bool isHeading = false, string badge = null, bool isOpened = false)
        {
            var item = new SidebarMenuItem
            {
                Title = title,
                IsFirst = isFirst,
                IsSelected = mvcUrlHelper.IsCurrentUrl(url),
                IsHeading = isHeading,
                IsOpen = isOpened
            };
            if (!string.IsNullOrEmpty(icon)) item.Icon = icon;
            if (!string.IsNullOrEmpty(url)) item.Url = url;
            if (!string.IsNullOrEmpty(badge)) item.BadgeText = badge;
            return item;
        }
    }
}