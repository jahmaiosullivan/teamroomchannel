using System;
using System.Collections.Generic;
using HobbyClue.Data.Models;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;

namespace HobbyClue.Business.Services
{
    public interface IScheduleService
    {
        IEnumerable<DateTime> GetOccurrences(Event ev, DateRange range = null, DateTime? afterDate = null, DateTime? untilDate = null);
    }

    public class ScheduleService : IScheduleService
    {
        public IEnumerable<DateTime> GetOccurrences(Event ev, DateRange range = null, DateTime? afterDate = null, DateTime? untilDate = null)
        {
            if (ev.Frequency == (int) FrequencyTypeEnum.None)
                return new List<DateTime> {ev.OneTimeOnlyEventDate ?? ev.StartDateTime.Value};

            var schedule = new Schedule(new RecurringEvent
            {
                Frequency = ev.Frequency == (int)FrequencyTypeEnum.Daily ? (int)FrequencyTypeEnum.Weekly : ev.Frequency,
                DaysOfWeek = ev.DaysOfWeek,
                DayOfMonth = ev.DayOfMonth,
                StartDateTime = afterDate,
                EndDateTime = untilDate,
                MonthlyInterval = ev.MonthlyInterval,
                OneTimeOnlyEventDate = ev.OneTimeOnlyEventDate,
                RepeatInterval = ev.RepeatInterval
            });
            if (range == null) {
                // give me all the upcoming dates for the next year
                var currentLocalTime = DateTime.UtcNow.ToLocalTime();
                range = new DateRange
                {
                    StartDateTime = currentLocalTime,
                    EndDateTime = currentLocalTime.AddYears(1)
                };
            }

            var occurrences = schedule.Occurrences(range);
            return occurrences;
        }
    }
}
