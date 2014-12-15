using AutoMapper;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings
{
    public class VoteMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Vote, VoteModel>()
                .ForMember(d => d.UserId, opts => opts.MapFrom(s => s.CreatedBy))
                .ForMember(d => d.Vote, opts => opts.MapFrom(s => (VoteType)s.Value));

            CreateMap<VoteModel, Vote>()
                .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                .ForMember(d => d.Value, opts => opts.MapFrom(s => (int)s.Vote))
                .ForMember(d => d.CardId, opts => opts.MapFrom(s => s.CardId))
                .ForMember(d => d.CreatedBy, opts => opts.MapFrom(s => s.UserId));
        }
    }
}