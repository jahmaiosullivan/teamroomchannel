using System;

namespace HobbyClue.Web.ViewModels
{
    public class PostCommentViewModel
    {
        public int CommentId { get; set; }
        public Guid CommentedBy { get; set; }
        public string CommentedByName { get; set; }
        public string CommentedByAvatar { get; set; }
        public string CommentedDate { get; set; }
        public string Message { get; set; }
        public int PostId { get; set; }
    }
}