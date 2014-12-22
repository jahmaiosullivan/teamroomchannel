using System.Collections.Generic;
using HobbyClue.Data.Models;
using HobbyClue.Data.Repositories;
using Teamroom.Business.Providers;

namespace HobbyClue.Business.Services
{
    public interface IMessageService : IBaseService<Message>
    {
        IEnumerable<Message> Get(long questionId);
    }

    public class MessageService : BaseDapperService<Message>, IMessageService
    {
        private readonly IMessageRepository questionRepository;
        public MessageService(IMessageRepository repository, IUserProvider userProvider)
            : base(repository, userProvider)
        {
            questionRepository = repository;
        }

        public IEnumerable<Message> Get(long questionId)
        {
            return questionRepository.Get(questionId);
        }
    }
}
