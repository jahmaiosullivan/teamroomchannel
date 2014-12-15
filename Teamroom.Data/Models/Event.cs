using System;
using System.ComponentModel.DataAnnotations;
using HobbyClue.Common.Attributes;

namespace HobbyClue.Data.Models
{
    public class Event : ModelBase
    {
        public DateTime? OneTimeOnlyEventDate { get; set; }

        /// <summary>
        /// If this event occurs only after a certain date,
        /// or repeats every x weeks, set the date.
        /// </summary>
        public DateTime? StartDateTime { get; set; }

        /// <summary>
        /// If this event occurs only until a specific ending date, set the date.
        /// Note that if you set both the EndDateTime, and a NumberOfOccurrences,
        /// The actual end date of the event will be determined by the more restrictive of 
        /// the two values.
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// If this event has a yearly frequency then the anniversary
        /// describes the fixed year after year month and day of recurrence.
        /// </summary>
        public int AnniversaryMonth { get; set; }
        public int AnniversaryDay { get; set; }

        // <summary>
        /// The start month for a schedule occurring during only part of the year
        /// </summary>
        public int MonthRangeStartMonth { get; set; }

        /// <summary>
        /// Optional start day of the month for fine-grained schedules (e.g., Apr 15).
        /// </summary>
        public int? MonthRangeStartDayMonth { get; set; }

        /// <summary>
        /// The end month for a schedule occurring during only part of the year
        /// </summary>
        public int MonthRangeEndMonth { get; set; }

        /// <summary>
        /// Optional end day of the month for fine-grained schedules (e.g., Oct 15).
        /// </summary>
        public int? MonthRangeEndDayMonth { get; set; }

        /// <summary>
        /// The one-time, daily, weekly, or monthly frequency of the event as a
        /// value of FrequencyTypeEnum (0, 1, 2, 4, 8, 16, 32, 64 or 128 only).
        /// </summary>
        public int Frequency { get; set; }

        /// <summary>
        /// If the frequency is daily, weekly, monthly, or yearly, 
        /// then set the interval of the event as an int.
        /// E.g., every second week == 2, every fourth day == 4.
        /// </summary>
        public int RepeatInterval { get; set; }

        /// <summary>
        /// If an event is quarterly, which quarter(s) does it fall in? 
        /// From QuarterEnum: First, Second, Third, Fourth 
        /// E.g., Second and Fourth quarters == 10 (2 + 8)
        /// </summary>
        public int QuarterInterval { get; set; }

        /// <summary>
        /// If the frequency is quarterly then the interval of the
        /// event as a flag attribute value of QuarterlyIntervalEnum.
        /// E.g., the first and last months of the quarter == 5
        /// </summary>
        public int QuarterlyInterval { get; set; }

        /// <summary>
        /// If the frequency is monthly then the interval of the
        /// event as a flag attribute value of MonthlyIntervalEnum.
        /// E.g., the first and third weeks of the month == 5
        /// </summary>
        public int MonthlyInterval { get; set; }

        /// <summary>
        /// The days of the week that the event occurs as a value
        /// of the DayOfWeekEnum flag attribute value. E.g., every
        /// day of the week is 127.
        /// </summary>
        public int DaysOfWeek { get; set; }

        /// <summary>
        /// This holds the number of occurrences that was last set with the 
        /// SetEndDateTimeForMaximumNumberOfOccurrences() function, for informational purposes only.
        /// If nothing has been set, this will contain null.
        /// </summary>
        public int? NumberOfOccurrencesThatWasLastSet { get; set; }
        
        /// <summary>
        /// A particular day of a month used for 'Monthly' frequency type.
        /// E.g., 1st day of every month, 10th day of every 2 months, etc.
        /// </summary>
        public int DayOfMonth { get; set; }


        [StringLength(250), Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(1024), Required]
        public string Location { get; set; }

        public Guid HostPerson { get; set; }

        public string Images { get; set; }

        public long GroupId { get; set; }

        public int? MaxAttendees { get; set; }

        public long ParentId { get; set; }
        
        [DapperIgnoreOnSaveOrUpdate]
        public string GroupName { get; set; }

        [DapperIgnoreOnSaveOrUpdate]
        public string GroupUrl { get; set; }
    }
}
