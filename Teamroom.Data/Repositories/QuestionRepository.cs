using System.Collections.Generic;
using HobbyClue.Data.Dapper;
using HobbyClue.Data.Models;

namespace HobbyClue.Data.Repositories
{
    public interface IQuestionRepository : IDapperRepository<Question>
    {
        IEnumerable<Question> Get(long productId);
    }

    public class QuestionRepository : BaseDapperRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(IQueryManager queryManager) 
                    : base(queryManager)
        {
        }

        public IEnumerable<Question> Get(long productId)
        {
            var query = BaseQuery + " WHERE t1.ProductId = @ProductId and t1.IsActive = 1 Order by t1.CreatedDate desc";
            return _queryManager.ExecuteSql<Question>(query, new { @ProductId = productId });
        }
    }
}
