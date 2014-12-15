using AutoMapper;
using HobbyClue.Data.Azure;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings
{
    public class ImageInfoMappingProfile : Profile
    {
        protected override void Configure()
        {
            CreateMap<ImageInfo, ImageInfoViewModel>();
        }
    }
}