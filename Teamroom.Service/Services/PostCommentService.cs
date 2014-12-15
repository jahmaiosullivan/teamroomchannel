using System.Collections.Generic;
using System.Linq;
using HobbyClue.Business.Providers;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;

namespace HobbyClue.Business.Services
{
    public interface IPostCommentService : IBaseService<PostComment>
    {
        IEnumerable<PostComment> GetForPost(long postId);
    }

    public class PostCommentService : BaseDapperService<PostComment>, IPostCommentService
    {
        private readonly IPostCommentRepository repository;
        private readonly IUserService userService;
         public PostCommentService(IPostCommentRepository repository, IUserProvider userProvider, IUserService userService) : base(repository, userProvider)
         {
             this.repository = repository;
             this.userService = userService;
         }

        public IEnumerable<PostComment> GetForPost(long postId)
        {
            var comments = repository.GetForPost(postId).ToList();
            foreach (var post in comments)
            {
                post.UserProfile = userService.GetById(post.CommentedBy);
            }
            return comments;
        }
    }
}
