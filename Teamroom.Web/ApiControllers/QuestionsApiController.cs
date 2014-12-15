using System.Net;
using System.Web;
using System.Web.Http;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.Controllers.Attributes;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ApiControllers
{
    public class QuestionsApiController : ApiController
    {
        private readonly IMappingEngine mappingEngine;
        private readonly IQuestionService questionService;
        private readonly IMessageService messageService;
        public QuestionsApiController(IMappingEngine mappingEngine, IQuestionService questionService, IMessageService messageService)
        {
            this.mappingEngine = mappingEngine;
            this.questionService = questionService;
            this.messageService = messageService;
        }

        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public QuestionViewModel Post(Question question)
        {
            if (ModelState.IsValid)
            {
                var savedQuestion = questionService.Save(question);
                var model = mappingEngine.Map<Question, QuestionViewModel>(savedQuestion);
                return model;
            }
            throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request");
        }

        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public MessageViewModel Reply(Message reply)
        {
            if (ModelState.IsValid)
            {
                var savedReply = messageService.Save(reply);
                var model = mappingEngine.Map<Message, MessageViewModel>(savedReply);
                return model;
            }
            throw new HttpException((int)HttpStatusCode.BadRequest, "Bad request");
        }
    }
}
