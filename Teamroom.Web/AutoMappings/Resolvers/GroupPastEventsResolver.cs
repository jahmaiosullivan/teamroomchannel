using System.Collections.Generic;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Common.Models;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class GroupPastEventsResolver : ValueResolver<long, IEnumerable<EventViewModel>>
    {
        private readonly IEventService eventService;
        private readonly IMappingEngine mappingEngine;
        public GroupPastEventsResolver(IEventService eventService, IMappingEngine mappingEngine)
        {
            this.eventService = eventService;
            this.mappingEngine = mappingEngine;
        }

        protected override IEnumerable<EventViewModel> ResolveCore(long groupId)
        {
            return CreateRecurringInstanceIfNotExistsYet(groupId);
        }

        private IEnumerable<EventViewModel> CreateRecurringInstanceIfNotExistsYet(long groupId)
        {
            var events = eventService.GetPastForGroup(groupId).EmptyListIfNull();
            return mappingEngine.Map<IEnumerable<Event>, List<EventViewModel>>(events);
        }
    }
}