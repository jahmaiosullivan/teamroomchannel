using System.Collections.Generic;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.ModelBuilders
{
    public interface IEventsModelBuilder
    {
        IEnumerable<EventViewModel> Build(long groupId);
        IEnumerable<EventViewModel> Build();
    }

    public class EventsModelBuilder : IEventsModelBuilder
    {
        private readonly IEventService eventService;
        private readonly IMappingEngine mappingEngine;

        public EventsModelBuilder(IEventService eventService, IMappingEngine mappingEngine)
        {
            this.eventService = eventService;
            this.mappingEngine = mappingEngine;
        }

        public IEnumerable<EventViewModel> Build(long groupId)
        {
            var events = eventService.GetUpcomingForGroup(groupId);
            var eventModels = mappingEngine.Map<IEnumerable<Event>, List<EventViewModel>>(events);
            return eventModels;
        }

        public IEnumerable<EventViewModel> Build()
        {
            var events = eventService.FindAll();
            var eventModels = mappingEngine.Map<IEnumerable<Event>, List<EventViewModel>>(events);
            return eventModels;
        }
    }
}