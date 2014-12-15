using System;
using System.Collections.Generic;
using System.Linq;
using HobbyClue.Common.Extensions;
using ScheduleWidget.Enums;

namespace HobbyClue.Web.ViewModels
{
    public class EventViewModel : BaseViewModel
    {
        public EventViewModel()
        {
            Hosts = new List<UserViewModel>();
            Attendees = new List<RSVPViewModel>();
            Posts = new List<PostViewModel>();
            DaysOfWeek = new List<int>();
        }
        
        public long Id { get; set; }
        public string Title { get; set; }

        public string StartDateDisplayText
        {
            get { return StartDate.FormatLongDateOnly(); }
        }

        public string StartTime
        {
            get { return StartDate.FormatTimeOnly(); }
        }

        public DateTime StartDate { get; set; }
        public string EndTime { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int MaxAttendees { get; set; }
        public IEnumerable<UserViewModel> Hosts { get; set; }
        public IEnumerable<RSVPViewModel> Attendees { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }
        public string Image { get; set; }
        public string NewComment { get; set; }
        public string GroupName { get; set; }
        public string GroupUrl { get; set; }
        public FrequencyTypeEnum Frequency { get; set; }
        public string Mode { get; set; }
        public IList<int> DaysOfWeek { get; set; }
        public List<DateTime> Occurrences { get; set; }

        public List<string> OccurrencesFormatted
        {
            get
            {
                return Occurrences.Select(occurence => occurence.FormatLongDateOnly()).ToList();
            }
        }

        public int FirstDayOfWeek
        {
            get { return DaysOfWeek.Any() ? DaysOfWeek[0] : -1; }
        }

        public int MonthlyWeekNum { get; set; }
        public int DayOfMonth { get; set; }
        public long ParentId { get; set; }
    }
}