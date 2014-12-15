using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IPostCommentRepository : IDapperRepository<PostComment>
    {
        IEnumerable<PostComment> GetForPost(long postId);
    }

    public class PostCommentRepository : BaseDapperRepository<PostComment>, IPostCommentRepository
    {
        public PostCommentRepository(IQueryManager queryManager)
            : base(queryManager)
        {
        }

        public IEnumerable<PostComment> GetForPost(long postId)
        {
            var sql = BaseQuery + " WHERE PostId = @postId";
            var result = _queryManager.ExecuteSql<PostComment>(sql, new { postId = postId });
            return result;
        }
    }
}
