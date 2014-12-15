using System;
using System.Linq;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using HobbyClue.Tests.Helpers;
using ScheduleWidget.Enums;
using ScheduleWidget.ScheduledEvents;
using Xunit;

namespace HobbyClue.Tests.HobbyClue.Service
{
    public class ScheduleServiceFacts
    {
        public class GetOccurrences
        {
            private readonly TestableScheduleService scheduleService = TestableScheduleService.Create();

            [Fact]
            public void GetsSpecificDateForEvent()
            {
                var ev = new Event
                {
                    Frequency = (int)FrequencyTypeEnum.None,
                    OneTimeOnlyEventDate = new DateTime(2015, 1, 3, 3, 45,0)
                };

                var occurrencesForTheYear = scheduleService.ClassUnderTest.GetOccurrences(ev).ToList();

                Assert.Equal(occurrencesForTheYear[0], ev.OneTimeOnlyEventDate);
            }
            
            [Fact]
            public void GetsMonthlySpecificDayOccurrences()
            {
                var ev = new Event
                {
                    Frequency = (int)FrequencyTypeEnum.Monthly,
                    DayOfMonth = 11,
                    StartDateTime = new DateTime(2014,11,10)
                };
                var occurrencesForTheYear = scheduleService.ClassUnderTest.GetOccurrences(ev, new DateRange
                {
                    StartDateTime = ev.StartDateTime.Value,
                    EndDateTime = ev.StartDateTime.Value.AddYears(1)
                }).ToList();

                Assert.Equal(occurrencesForTheYear[0], new DateTime(2014, 11, 11));
                Assert.Equal(occurrencesForTheYear[1], new DateTime(2014, 12, 11));
                Assert.Equal(occurrencesForTheYear[2], new DateTime(2015, 1, 11));
                Assert.Equal(occurrencesForTheYear[3], new DateTime(2015, 2, 11));
                Assert.Equal(occurrencesForTheYear[4], new DateTime(2015, 3, 11));
                Assert.Equal(occurrencesForTheYear[5], new DateTime(2015, 4, 11));
                Assert.Equal(occurrencesForTheYear[6], new DateTime(2015, 5, 11));
                Assert.Equal(occurrencesForTheYear[7], new DateTime(2015, 6, 11));
                Assert.Equal(occurrencesForTheYear[8], new DateTime(2015, 7, 11));
                Assert.Equal(occurrencesForTheYear[9], new DateTime(2015, 8, 11));
                Assert.Equal(occurrencesForTheYear[10], new DateTime(2015, 9, 11));
                Assert.Equal(occurrencesForTheYear[11], new DateTime(2015, 10, 11));
            }

            [Fact]
            public void GetsMonthlySpecificDayOccurrencesWithinDateRange()
            {
                var ev = new Event
                {
                    Frequency = (int)FrequencyTypeEnum.Monthly,
                    DayOfMonth = 11,
                    StartDateTime = new DateTime(2014, 11, 10)
                };
                var occurrencesForTheYear = scheduleService.ClassUnderTest.GetOccurrences(ev, new DateRange
                {
                    StartDateTime = ev.StartDateTime.Value,
                    EndDateTime = ev.StartDateTime.Value.AddYears(2)
                }).ToList();

                Assert.Equal(occurrencesForTheYear.Count, 24);
            }
            
            [Fact]
            public void UsesFrequencyTypeToDetermineOccurrenceEvenIfUnrelatedFieldsAreSet()
            {
                var ev = new Event
                {
                    Frequency = (int)FrequencyTypeEnum.Monthly,
                    DayOfMonth = 11,
                    StartDateTime = new DateTime(2014, 11, 10),
                    DaysOfWeek = (int) (DayOfWeekEnum.Mon | DayOfWeekEnum.Sun | DayOfWeekEnum.Thu),
                    OneTimeOnlyEventDate = DateTime.UtcNow
                };
                var occurrencesForTheYear = scheduleService.ClassUnderTest.GetOccurrences(ev, new DateRange
                {
                    StartDateTime = ev.StartDateTime.Value,
                    EndDateTime = ev.StartDateTime.Value.AddYears(1)
                }).ToList();

                Assert.Equal(occurrencesForTheYear[0], new DateTime(2014, 11, 11));
                Assert.Equal(occurrencesForTheYear[1], new DateTime(2014, 12, 11));
                Assert.Equal(occurrencesForTheYear[2], new DateTime(2015, 1, 11));
                Assert.Equal(occurrencesForTheYear[3], new DateTime(2015, 2, 11));
                Assert.Equal(occurrencesForTheYear[4], new DateTime(2015, 3, 11));
                Assert.Equal(occurrencesForTheYear[5], new DateTime(2015, 4, 11));
                Assert.Equal(occurrencesForTheYear[6], new DateTime(2015, 5, 11));
                Assert.Equal(occurrencesForTheYear[7], new DateTime(2015, 6, 11));
                Assert.Equal(occurrencesForTheYear[8], new DateTime(2015, 7, 11));
                Assert.Equal(occurrencesForTheYear[9], new DateTime(2015, 8, 11));
                Assert.Equal(occurrencesForTheYear[10], new DateTime(2015, 9, 11));
                Assert.Equal(occurrencesForTheYear[11], new DateTime(2015, 10, 11));
            }

        }


        public class TestableScheduleService : Facts<ScheduleService>
        {
            public static TestableScheduleService Create()
            {
                var service = new TestableScheduleService();
                return service;
            }
        }
    }
}
