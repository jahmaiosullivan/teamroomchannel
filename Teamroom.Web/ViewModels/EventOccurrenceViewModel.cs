using System;
using System.Collections.Generic;

namespace HobbyClue.Web.ViewModels
{
    public class EventOccurrenceViewModel
    {
        public long Id { get; set; }
        public long EventId { get; set; }
        public string Date { get; set; }
        public IEnumerable<RSVPViewModel> Attendees { get; set; }
    }
}