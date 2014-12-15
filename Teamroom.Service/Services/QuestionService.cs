using System.Collections.Generic;
using HobbyClue.Business.Providers;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;

namespace HobbyClue.Business.Services
{
    public interface IQuestionService : IBaseService<Question>
    {
        IEnumerable<Question> Get(long productId);
    }

    public class QuestionService : BaseDapperService<Question>, IQuestionService
    {
        private readonly IQuestionRepository questionRepository;
        public QuestionService(IQuestionRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
            questionRepository = repository;
        }

        public IEnumerable<Question> Get(long productId)
        {
            return questionRepository.Get(productId);
        }

    }
}
