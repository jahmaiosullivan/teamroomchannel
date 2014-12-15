namespace HobbyClue.Web.ViewModels
{
    public class EventPostViewModel : PostViewModel
    {
        public long EventId { get; set; }
    }

    public class GroupPostViewModel : PostViewModel
    {
        public long GroupId { get; set; }
    }
}