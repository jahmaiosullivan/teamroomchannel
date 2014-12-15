using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.Mvc.Html;
using Newtonsoft.Json.Serialization;

namespace HobbyClue.Web.ViewModels
{
    public class CamelcaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return Char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        }
    }

    public static class ObjectExtensions
    {
        public static string ToJson(this object obj)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelcaseContractResolver(),
                DateFormatString = "MM/dd/yyyy hh:mm:ss"
            };
            var js = JsonSerializer.Create(settings);
            var jw = new StringWriter();
            js.Serialize(jw, obj);
            return jw.ToString();
        }

        public static MvcHtmlString DropDownList(this HtmlHelper helper,
            string name, Dictionary<int, string> dictionary)
        {
            var selectListItems = new SelectList(dictionary, "Key", "Value");
            return helper.DropDownList(name, selectListItems);
        }


        public static object ConvertTo<T>(this object obj) 
        {
            var converter = TypeDescriptor.GetConverter(obj.GetType());
            if (converter.CanConvertTo(typeof(T)))
            {
                return converter.ConvertTo(obj, typeof(T));
            }
            return obj.ToString();
        }

        public static T ConvertFrom<T>(this object obj) where T : new()
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertFrom(obj.GetType()))
            {
                return (T)converter.ConvertFrom(obj);
            }
            return new T();
        }


        public static string UpperCaseFirstLetter(this string text)
        {
            var trimmedText = text.Trim();
            if (!string.IsNullOrEmpty(trimmedText.Trim()))
                return char.ToUpper(trimmedText[0]) + trimmedText.Substring(1).ToLower();
            return trimmedText;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element)));
        }
    }

}