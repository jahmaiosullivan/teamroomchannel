using System;
using System.Globalization;
using System.Linq;

namespace HobbyClue.Common.Extensions
{
    public enum TimeZones
    {
        PacificStandardTime = 0
    }

    public static class DateHelper
    {
        public static DateTime ConvertToUTC(this DateTime date)
        {
            var unspecifiedTimezoneDate = new DateTime(date.Ticks, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(unspecifiedTimezoneDate, TimeZoneInfo.Local);
            //var date1 = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
            //var tz = TimeZoneInfo.GetSystemTimeZones().First(x => x.Id == GetTimeZoneId(timezone));
            //var utcTime = TimeZoneInfo.ConvertTimeToUtc(date1, tz);
            //return utcTime;
        }
        
        private static string GetTimeZoneId(TimeZones timeZone)
        {
            var timeZoneId = String.Empty;
            switch (timeZone)
            {
                case TimeZones.PacificStandardTime:
                    timeZoneId = "Pacific Standard Time";
                    break;
            }
            return timeZoneId;
        }

        public static string PaddedTicks(this DateTime value)
        {
            return value.Ticks.ToString().PadLeft(20, '0');
        }

        private static DateTime _utcNow = DateTime.MinValue;

        public static DateTime UtcNow
        {
            get
            {
                if (_utcNow == DateTime.MinValue)
                    _utcNow = DateTime.UtcNow;
                return _utcNow;
            }
            set
            {
                _utcNow = value;
            }
        }

        public static string FormatStartToEndTimes(DateTime startdate, DateTime endDate)
        {
            return string.Format("{0} to {1}", startdate.ToString("h:mm tt"), endDate.ToString("h:mm tt"));
        }

        public static string FormatDateOnly(this DateTime date)
        {
            return date.ToString("MM/d/yyyy");
        }

        public static string FormatLongDateOnly(this DateTime date)
        {
            return date.ToString("ddd MMM d, yyyy");
        }

        public static DateTime ConvertFromUtcToTimeZone(this DateTime date)
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return ConvertFromUtcToTimeZone(date, zone);
        }

        public static DateTime ConvertFromUtcToTimeZone(this DateTime date, TimeZoneInfo timeZoneToConvertTo)
        {
            if(timeZoneToConvertTo == null) timeZoneToConvertTo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(date, timeZoneToConvertTo);
        }

        public static DateTime UpdateTime(this DateTime date, int hours, int minutes, int seconds)
        {
            var ts = new TimeSpan(hours, minutes, seconds);
            date = date.Date + ts;
            return date;
        }

        public static string FormatTimeOnly(this DateTime date)
        {
            return date.ToString("h:mm tt");
        }

        public static string FormatZtime(this DateTime date)
        {
            return date.ToString("yyyy-dd-MMTHH:mm:ss Z");
        }

        public static string FormatDateTime(this DateTime date)
        {
            return date.ToString(DefaultDateTimeFormat);
        }

        private const string DefaultDateTimeFormat = "ddd MMM d, yyyy h:mm tt";

        public static DateTime ConvertFormattedStringToDateTime(this string dateString)
        {
            DateTime dt;
            DateTime.TryParseExact(dateString, DefaultDateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            return dt;
        }
    }
}
