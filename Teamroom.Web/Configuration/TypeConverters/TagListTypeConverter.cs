using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using HobbyClue.Data.Models;
using HobbyClue.Web.ViewModels;

namespace HobbyClue.Web.Configuration.TypeConverters
{
    public class TagListTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var tags = value.ToString().Split(',').ToList();
                var tagsViewModel = new TagListViewModel();
                tagsViewModel.AddRange(tags.Select(t => new Tag { Name = t }));
                return tagsViewModel;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(TagListViewModel) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            var model = value as TagListViewModel;
            if (model != null)
            {
                return String.Join(",", model.Select(x => x.Name).ToList());
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}