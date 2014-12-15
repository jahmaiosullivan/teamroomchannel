using System;
using System.Web.Http;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.Controllers.Attributes;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ApiControllers
{
    public class IntrosApiController : ApiController
    {
        private readonly IIntroService introService;
        private readonly IMappingEngine mappingEngine;
        public IntrosApiController(IIntroService introService, IMappingEngine mappingEngine)
        {
            this.introService = introService;
            this.mappingEngine = mappingEngine;
        }

        [HttpPost]
        [ValidateApiAntiForgeryToken]
        public IntroViewModel Create(Intro newIntro)
        {
            var savedIntro =  introService.Save(newIntro);
            return mappingEngine.Map<Intro, IntroViewModel>(savedIntro);
        }
    }
}
