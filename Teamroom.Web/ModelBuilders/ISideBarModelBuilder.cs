using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ModelBuilders
{
    public interface ISideBarModelBuilder
    {
        AdminSidebarViewModel BuildAdminSideBar();
        SideBarViewModel BuildModel();
    }
}