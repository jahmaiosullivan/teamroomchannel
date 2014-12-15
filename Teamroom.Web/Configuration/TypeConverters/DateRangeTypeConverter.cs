using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.Configuration.TypeConverters
{
    public class DateRangeTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var toAndFromDates = ((string) value).Split(',').ToList();
                if (toAndFromDates.Count == 2)
                {
                    DateTime toDate;
                    DateTime fromDate;
                    DateTime.TryParse(toAndFromDates[0], out fromDate);
                    DateTime.TryParse(toAndFromDates[1], out toDate);

                    return new DateRangeViewModel
                    {
                        From = fromDate,
                        To = toDate
                    };
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}