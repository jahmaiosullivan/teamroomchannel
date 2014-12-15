using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using HobbyClue.Business.Services;
using HobbyClue.Common.Models;
using HobbyClue.Data.Models;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class EventOccurencesResolver : ValueResolver<Event, IList<DateTime>>
    {
        private readonly IScheduleService scheduleService;
        public EventOccurencesResolver(IScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        protected override IList<DateTime> ResolveCore(Event source)
        {
            var occurrences = scheduleService.GetOccurrences(source).Take(5).EmptyListIfNull();
            return occurrences;
        }
    }
}