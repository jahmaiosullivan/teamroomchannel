using System;
using AutoMapper;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;
using HobbyClue.Web.AutoMappings.Resolvers;

namespace HobbyClue.Web.AutoMappings
{
    public class IntroMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Intro, IntroViewModel>()
                .ForMember(d => d.Images, opts => opts.MapFrom(s => s.Images.Split(',')))
                .ForMember(d => d.CreatedBy, opts => opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.CreatedBy ?? Guid.Empty))
                .ForMember(d => d.LastUpdatedBy, opts =>  opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.LastUpdatedBy ?? Guid.Empty));
        }
    }
}