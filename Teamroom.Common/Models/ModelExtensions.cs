using System;
using System.Collections.Generic;
using System.Linq;

namespace HobbyClue.Common.Models
{
    public static class ModelExtensions
    {
        public static string FormatDate(this DateTime date)
        {
            return date.ToString("MMM dd at h:mm");
        }

        public static List<string> SplitIntoCommaSeparatedList(this string itemsString)
        {
            return !string.IsNullOrEmpty(itemsString) ? itemsString.Split(',').ToList() : new List<string>();
        }

        public static List<Guid> SplitStringIntoGuidList(this string itemsString)
        {
            return !string.IsNullOrEmpty(itemsString) ? itemsString.Split(',').Select(x => new Guid(x)).ToList() : new List<Guid>();
        }

        public static IList<T> EmptyListIfNull<T>(this IList<T> list)
        {
            return list ?? new List<T>();
        }

        public static IList<T> EmptyListIfNull<T>(this IEnumerable<T> list)
        {
            return list == null ? new List<T>() : list.ToList();
        }

        public static void AddUnique<T>(this IList<T> list, T item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }


        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            if (source != null)
            {
                var knownKeys = new HashSet<TKey>();
                foreach (var element in source)
                {
                    if (knownKeys.Add(keySelector(element)))
                    {
                        yield return element;
                    }
                }
            }
        }

        public static List<List<TSource>> SplitList<TSource>(this IEnumerable<TSource> source, int nSize)
        {
            var result = new List<List<TSource>>();
            var divisions = Math.Ceiling((double)source.Count() / nSize);
            for (var i = 0; i < divisions; i++)
            {
                result.Add(source.Skip(i * nSize).Take(nSize).ToList());
            }
            return result;
        }


        public static String SubstringToWordEnd(this String input, int length)
        {
            if (input == null || input.Length < length)
                return input;
            var iNextSpace = input.LastIndexOf(" ", length, StringComparison.Ordinal);
            return input.Substring(0, (iNextSpace > 0) ? iNextSpace : length).Trim();
        }

    }
}
