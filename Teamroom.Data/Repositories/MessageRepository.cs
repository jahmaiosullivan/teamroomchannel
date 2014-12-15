using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IMessageRepository : IDapperRepository<Message>
    {
        IEnumerable<Message> Get(long questionId);
    }

    public class MessageRepository : BaseDapperRepository<Message>, IMessageRepository
    {
        public MessageRepository(IQueryManager queryManager) 
                    : base(queryManager)
        {
        }

        public IEnumerable<Message> Get(long questionId)
        {
            var query = BaseQuery + " WHERE t1.QuestionId = @QuestionId and t1.IsActive = 1 Order by t1.CreatedDate asc";
            return _queryManager.ExecuteSql<Message>(query, new { @QuestionId = questionId });
        }
    }
}
