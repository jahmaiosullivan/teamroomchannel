using System;
using AutoMapper;
using HobbyClue.Data.Models;
using HobbyClue.Web.AutoMappings.Resolvers;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings
{
    public class CompanyMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Company, CompanyViewModel>()
                .ForMember(d => d.CommunityConversations, opts => opts.Ignore())
                .ForMember(d => d.MyConversations, opts => opts.Ignore())
                .ForMember(d => d.Intro, opts => opts.ResolveUsing<CompanyIntroResolver>())
                .ForMember(d => d.Team, opts => opts.Ignore())
                .ForMember(d => d.CreatedBy, opts => opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.CreatedBy ?? Guid.Empty))
                .ForMember(d => d.LastUpdatedBy, opts =>  opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.LastUpdatedBy ?? Guid.Empty));
        }
    }
}