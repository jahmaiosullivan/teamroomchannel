using System;
using AutoMapper;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;
using HobbyClue.Web.AutoMappings.Resolvers;

namespace HobbyClue.Web.AutoMappings
{
    public class MessageMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Message, MessageViewModel>()
                .ForMember(d => d.Children, opts => opts.Ignore())
                .ForMember(d => d.CreatedBy, opts => opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.CreatedBy ?? Guid.Empty))
                .ForMember(d => d.LastUpdatedBy, opts =>  opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.LastUpdatedBy ?? Guid.Empty));
        }
    }
}