using AutoMapper;
using StructureMap;

namespace HobbyClue.Web.AutoMappings
{
    public static class WebMapping
    {
        public static void Configure(IContainer container)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.ConstructServicesUsing(container.GetInstance);
                cfg.AddProfile<ImageInfoMappingProfile>();
                cfg.AddProfile<VoteMappingProfile>();
                cfg.AddProfile<TagMappingProfile>();
                cfg.AddProfile<EventMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<IntroMappingProfile>();
                cfg.AddProfile<CompanyMappingProfile>();
                cfg.AddProfile<PostMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<QuestionMappingProfile>();
                cfg.AddProfile<MessageMappingProfile>();
            });
            Mapper.AssertConfigurationIsValid();
        }
    }
}