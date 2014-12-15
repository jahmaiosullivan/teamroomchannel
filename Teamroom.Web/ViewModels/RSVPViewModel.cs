namespace HobbyClue.Web.ViewModels
{
    public class RSVPViewModel
    {
        public UserViewModel User { get; set; }
        public bool IsAttending { get; set; }
        public bool SendNotifications { get; set; }
        public bool IsWaitlisted { get; set; }
        public long EventId { get; set; }
    }
}