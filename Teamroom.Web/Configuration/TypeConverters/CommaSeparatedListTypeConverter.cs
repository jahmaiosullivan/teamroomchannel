using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using HobbyClue.Web.Models;

namespace HobbyClue.Web.Configuration.TypeConverters
{
    public class CommaSeparatedListTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var strings =  new CommaSeparatedList();
            if (value != null)
            {
                var valuesSplit = value.ToString().Split(',').ToList();
                strings.AddRange(valuesSplit);
                return strings;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(CommaSeparatedList) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var values = value as CommaSeparatedList;
            return values != null ? String.Join(",", values) : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}