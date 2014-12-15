using System.Web.Mvc;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ModelBuilders;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly IQuestionService questionService;
        private readonly IMappingEngine mappingEngine;
        private readonly IQuestionModelBuilder questionModelBuilder;
        public QuestionsController(IQuestionService questionService, IMappingEngine mappingEngine, IQuestionModelBuilder questionModelBuilder)
        {
            this.questionService = questionService;
            this.mappingEngine = mappingEngine;
            this.questionModelBuilder = questionModelBuilder;
        }

        public ActionResult Index(long id)
        {
            var question = questionService.GetById(id);
            var model = mappingEngine.Map<Question, QuestionViewModel>(question);
            model.Replies = questionModelBuilder.ThreadReplies(model.Replies);
            return View(model);
        }
    }
}