using System.Collections.Generic;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class QuestionRepliesResolver : ValueResolver<Question, IEnumerable<MessageViewModel>>
    {
        private readonly IMessageService messageService;
        private readonly IMappingEngine mappingEngine;
        public QuestionRepliesResolver(IMessageService messageService, IMappingEngine mappingEngine)
        {
            this.messageService = messageService;
            this.mappingEngine = mappingEngine;
        }

        protected override IEnumerable<MessageViewModel> ResolveCore(Question source)
        {
            var replies = messageService.Get(source.Id);
            return mappingEngine.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(replies);
        }
    }
}