using System;
using System.ComponentModel;
using HobbyClue.Web.Configuration.TypeConverters;

namespace HobbyClue.Web.ViewModels
{

    [TypeConverter(typeof(DateRangeTypeConverter))]
    public class DateRangeViewModel
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}