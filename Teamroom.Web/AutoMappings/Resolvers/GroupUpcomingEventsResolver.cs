using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Common.Extensions;
using HobbyClue.Common.Models;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;
using ScheduleWidget.Enums;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class GroupUpcomingEventsResolver : ValueResolver<long, IEnumerable<EventViewModel>>
    {
        private readonly IEventService eventService;
        private readonly IMappingEngine mappingEngine;
        public GroupUpcomingEventsResolver(IEventService eventService, IMappingEngine mappingEngine)
        {
            this.eventService = eventService;
            this.mappingEngine = mappingEngine;
        }

        protected override IEnumerable<EventViewModel> ResolveCore(long groupId)
        {
            var events = eventService.GetUpcomingForGroup(groupId).EmptyListIfNull();
            return CreateRecurringInstanceIfNotExistsYet(events);
        }

        private IEnumerable<EventViewModel> CreateRecurringInstanceIfNotExistsYet(IList<Event> events)
        {
            var eventModels = mappingEngine.Map<IEnumerable<Event>, List<EventViewModel>>(events);
            var result = new List<EventViewModel>();
            foreach (var ev in eventModels)
            {
                //If we already have an event instance of a recurring event, do not put parent event on the list
                if (ev.Frequency != (int) FrequencyTypeEnum.None && ev.ParentId == 0 &&
                    eventModels.Any(x => x.ParentId == ev.Id))
                    continue;

                if (ev.Frequency == (int)FrequencyTypeEnum.None || !ev.Occurrences.Any() || ev.ParentId > 0)
                {
                    result.Add(ev);
                    continue;
                }
                
                var firstOccurrenceDate = new DateTime(ev.Occurrences.First().UpdateTime(ev.StartDate.Hour, ev.StartDate.Minute, ev.StartDate.Second).Ticks, DateTimeKind.Unspecified);
                var eventInstance = events.FirstOrDefault(x => x.ParentId == ev.Id && x.StartDateTime == TimeZoneInfo.ConvertTimeToUtc(firstOccurrenceDate, TimeZoneInfo.Local));
                if (eventInstance != null) {
                    result.Add(ev);
                    continue;
                }

                //Create an event for this displayed item
                var savedEvent = eventService.CreateRecurringInstance(events.First(x => x.Id == ev.Id), firstOccurrenceDate);
                var savedEventModel = mappingEngine.Map<Event, EventViewModel>(savedEvent);
                result.Add(savedEventModel);
            }

            return result.OrderBy(x => x.StartDate);
        }

    }
}