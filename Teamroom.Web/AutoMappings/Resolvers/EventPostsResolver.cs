using System.Collections.Generic;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class EventPostsResolver : ValueResolver<Event, List<PostViewModel>>
    {
        private readonly IPostService postService;
        private readonly IMappingEngine mappingEngine;
        public EventPostsResolver(IMappingEngine mappingEngine, IPostService postService)
        {
            this.mappingEngine = mappingEngine;
            this.postService = postService;
        }

        protected override List<PostViewModel> ResolveCore(Event source)
        {
            return mappingEngine.Map<IEnumerable<Post>, List<PostViewModel>>(postService.GetForEvent(source.Id));
        }
    }
}