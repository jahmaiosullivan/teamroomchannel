using System.Linq;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class CompanyIntroResolver : ValueResolver<Company, IntroViewModel>
    {
        private readonly IIntroService introService;
        private readonly IMappingEngine mappingEngine;
        public CompanyIntroResolver(IIntroService introService, IMappingEngine mappingEngine)
        {
            this.introService = introService;
            this.mappingEngine = mappingEngine;
        }

        protected override IntroViewModel ResolveCore(Company source)
        {
            var intro = introService.FindAll().First();
            return mappingEngine.Map<Intro, IntroViewModel>(intro);
        }
    }
}