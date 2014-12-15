namespace HobbyClue.Web.ViewModels
{
    public enum SidebarOptions
    {
        Members,
        Upcoming,
        Past,
        Discussions,
        Reviews,
        Settings
    }

    public class SideBarViewModel
    {
        public SidebarOptions Selected { get; set; }
        public string GroupName { get; set; }
    }
}