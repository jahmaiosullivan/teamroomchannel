using System.Collections.Generic;
using AutoMapper;
using ScheduleWidget.Enums;

namespace HobbyClue.Web.AutoMappings.Resolvers
{
    public class DaysOfWeekResolver : ValueResolver<int, List<int>>
    {
        protected override List<int> ResolveCore(int source)
        {
            var result = new List<int>();
            var days = (DayOfWeekEnum) source;
            AddSelectedDayOfWeek(ref result, days, DayOfWeekEnum.Sun);
            AddSelectedDayOfWeek(ref result, days, DayOfWeekEnum.Mon);
            AddSelectedDayOfWeek(ref result, days, DayOfWeekEnum.Tue);
            AddSelectedDayOfWeek(ref result, days, DayOfWeekEnum.Wed);
            AddSelectedDayOfWeek(ref result, days, DayOfWeekEnum.Thu);
            AddSelectedDayOfWeek(ref result, days, DayOfWeekEnum.Fri);
            AddSelectedDayOfWeek(ref result, days, DayOfWeekEnum.Sat);
            return result;
        }

        private static void AddSelectedDayOfWeek(ref List<int> result, DayOfWeekEnum days, DayOfWeekEnum dayBeingChecked)
        {
            if ((days & dayBeingChecked) == dayBeingChecked)
            {
                result.Add((int)dayBeingChecked);
            }
        }
    }
}