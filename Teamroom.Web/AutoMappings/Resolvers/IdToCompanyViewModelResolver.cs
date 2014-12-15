using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class IdToCompanyViewModelResolver : ValueResolver<long, CompanyViewModel>
    {
        private readonly ICompanyService companyService;
        private readonly IMappingEngine mappingEngine;
        public IdToCompanyViewModelResolver(ICompanyService companyService, IMappingEngine mappingEngine)
        {
            this.companyService = companyService;
            this.mappingEngine = mappingEngine;
        }

        protected override CompanyViewModel ResolveCore(long source)
        {
            return mappingEngine.Map<Company,CompanyViewModel>(companyService.GetById(source));
        }
    }
}