using System;
using AutoMapper;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;
using HobbyClue.Web.AutoMappings.Resolvers;

namespace HobbyClue.Web.AutoMappings
{
    public class ProductMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.Company, opts => opts.ResolveUsing<IdToCompanyViewModelResolver>().FromMember(s => s.CompanyId))
                .ForMember(d => d.Questions, opts => opts.ResolveUsing<ProductQuestionsResolver>())
                .ForMember(d => d.CreatedBy, opts => opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.CreatedBy ?? Guid.Empty))
                .ForMember(d => d.LastUpdatedBy, opts =>  opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.LastUpdatedBy ?? Guid.Empty));
        }
    }
}