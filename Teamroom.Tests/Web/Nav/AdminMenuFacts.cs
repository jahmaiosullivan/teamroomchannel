using System.Linq;
using HobbyClue.Web.Nav;
using Xunit;

namespace HobbyClue.Tests.Web.Nav
{
   public class AdminMenuFacts
    {
       public class BuildAdminLinks
       {
           [Fact]
           public void SetsManageEventsLinkSelected()
           {
               var result = new AdminMenu(LinkType.AdminEvents);

               Assert.True(result.First(x => x.Id == (int)LinkType.AdminEvents).IsSelected);
           }

           [Fact]
           public void SetsAllOtherLinksOtherThanManageEventsUnselectedWhenSelectedManageEvents()
           {
               var result = new AdminMenu(LinkType.AdminEvents).ToList();

               Assert.False(result.First(x => x.Id == (int)LinkType.AdminDashboard).IsSelected);
               Assert.False(result.First(x => x.Id == (int)LinkType.AdminUsers).IsSelected);
           }

           [Fact]
           public void SetsManageUsersLinkSelected()
           {
               var result = new AdminMenu(LinkType.AdminUsers);

               Assert.True(result.First(x => x.Id == (int)LinkType.AdminUsers).IsSelected);
           }

           [Fact]
           public void SetsAllOtherLinksOtherThanManageUsersUnselectedWhenSelectedManageUsers()
           {
               var result = new AdminMenu(LinkType.AdminUsers).ToList();

               Assert.False(result.First(x => x.Id == (int)LinkType.AdminDashboard).IsSelected);
               Assert.False(result.First(x => x.Id == (int)LinkType.AdminEvents).IsSelected);
           }

           [Fact]
           public void SetsAdminDashboardLinkSelected()
           {
               var result = new AdminMenu(LinkType.AdminDashboard);

               Assert.True(result.First(x => x.Id == (int)LinkType.AdminDashboard).IsSelected);
           }

           [Fact]
           public void SetsAllOtherLinksOtherThanAdminDashboardUnselectedWhenSelectedAdminDashboard()
           {
               var result = new AdminMenu(LinkType.AdminDashboard).ToList();

               Assert.False(result.First(x => x.Id == (int)LinkType.AdminUsers).IsSelected);
               Assert.False(result.First(x => x.Id == (int)LinkType.AdminEvents).IsSelected);
           }
       }

    }
}
