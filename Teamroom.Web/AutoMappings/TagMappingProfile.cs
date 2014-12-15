using AutoMapper;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings
{
    public class TagMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<Tag, TagViewModel>()
                 .ForMember(d => d.Id, opts => opts.MapFrom(s => s.Id))
                 .ForMember(d => d.Name, opts => opts.MapFrom(s => s.Name))
                 .ForMember(d => d.Description, opts => opts.MapFrom(s => s.Description))
                 .ForMember(d => d.DateCreated, opts => opts.Ignore())
                 .ForMember(d => d.IsSelected, opts => opts.UseValue(false));

        }
    }
}