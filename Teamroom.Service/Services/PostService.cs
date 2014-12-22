using System.Collections.Generic;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public interface IPostService : IBaseService<Post>
    {
        IEnumerable<Post> GetForEvent(long eventId);
        IEnumerable<Post> GetPostsForGroup(long groupId);
    }

    public class PostService : BaseDapperService<Post>, IPostService
    {
        private readonly IPostRepository postRepository;
        public PostService(IPostRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
            postRepository = repository;
        }

        public IEnumerable<Post> GetForEvent(long eventId)
        {
            return postRepository.GetPostsForEvent(eventId);
        }

        public IEnumerable<Post> GetPostsForGroup(long groupId)
        {
            return postRepository.GetPostsForGroup(groupId);
        }
    }
}
