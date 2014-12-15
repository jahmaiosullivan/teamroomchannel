using System;
using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            PostComments = new List<PostCommentViewModel>();
        }

        public string Message { get; set; }
        public string  PostedBy { get; set; }
        public string PostedByName { get; set; }
        public string PostedByAvatar { get; set; }
        public DateTime PostedDate { get; set; }
        public int PostId { get; set; }
        public IEnumerable<PostCommentViewModel> PostComments { get; set; }
        public string NewCommentMessage { get; set; }
    }
}