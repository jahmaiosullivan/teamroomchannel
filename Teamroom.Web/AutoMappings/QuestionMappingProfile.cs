using System;
using AutoMapper;
using HobbyClue.Common.Helpers;
using HobbyClue.Data.Models;
using HobbyClue.Web.AutoMappings.Resolvers;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings
{
    public class QuestionMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Question, QuestionViewModel>()
                .ForMember(d => d.Replies, opts => opts.ResolveUsing<QuestionRepliesResolver>())
                .ForMember(d => d.Body, opts => opts.MapFrom(s => !string.IsNullOrEmpty(s.Body) ? s.Body : string.Empty))
                .ForMember(d => d.BodySummary, opts => opts.MapFrom(s => !string.IsNullOrEmpty(s.Body) ? StringHelper.TrimWithEllipses(s.Body, 200) : string.Empty))
                .ForMember(d => d.State, opts => opts.MapFrom(s => s.State.ToString().ToLower()))
                .ForMember(d => d.CreatedBy, opts => opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.CreatedBy ?? Guid.Empty))
                .ForMember(d => d.LastUpdatedBy, opts =>  opts.ResolveUsing<IdToUserViewModelResolver>().FromMember(s => s.LastUpdatedBy ?? Guid.Empty));
        }
    }
}