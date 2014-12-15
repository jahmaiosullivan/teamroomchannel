using System;
using HobbyClue.Common.Attributes;

namespace HobbyClue.Data.Models
{
    public class EventOccurrence
    {
        [AutoSuppliedFromDatabase]
        [PrimaryKey]
        public virtual long Id { get; set; }

        public long EventId { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
    }
}