using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IPostRepository : IDapperRepository<Post>
    {
        IEnumerable<Post> GetPostsForEvent(long eventId);
        IEnumerable<Post> GetPostsForGroup(long groupId);
    }

    public class PostRepository : BaseDapperRepository<Post>, IPostRepository
    {
        public PostRepository(IQueryManager queryManager)
            : base(queryManager)
        {
        }

        public IEnumerable<Post> GetPostsForEvent(long eventId)
        {
            var sql = BaseQuery + " WHERE t1.PostId in (Select PostId from [EventPosts] where EventId = @EventId) order by t1.PostedDate asc";
            var events = _queryManager.ExecuteSql<Post>(sql, new { @EventId = eventId });
            return events;
        }

        public IEnumerable<Post> GetPostsForGroup(long groupId)
        {
            var sql = BaseQuery + " WHERE t1.PostId in (Select PostId from [GroupPosts] where GroupId = @GroupId) order by t1.PostedDate asc";
            var events = _queryManager.ExecuteSql<Post>(sql, new { @GroupId = groupId });
            return events;
        }
    }
}
