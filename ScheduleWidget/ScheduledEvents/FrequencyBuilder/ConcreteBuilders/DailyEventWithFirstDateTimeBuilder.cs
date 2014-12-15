﻿using System;
using ScheduleWidget.Enums;
using ScheduleWidget.TemporalExpressions;

namespace ScheduleWidget.ScheduledEvents.FrequencyBuilder.ConcreteBuilders
{
    public class DailyEventWithFirstDateTimeBuilder : IEventFrequencyBuilder
    {
        private readonly RecurringEvent _event;

        public DailyEventWithFirstDateTimeBuilder(RecurringEvent aEvent)
        {
            //Assigning default value to day interval if the value is 0.
            if (aEvent.RepeatInterval == 0)
                aEvent.RepeatInterval = 1;

            _event = aEvent;
        }

        public UnionTE Create()
        {
            var union = new UnionTE();

            var firstDateTime = _event.StartDateTime ?? DateTime.Now;

            foreach (DayOfWeekEnum day in Enum.GetValues(typeof(DayOfWeekEnum)))
            {
                union.Add(new DayIntervalTE(_event.RepeatInterval, firstDateTime));
            }

            return union;
        }
    }
}