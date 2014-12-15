using System;
using System.Text.RegularExpressions;
using System.Web;

namespace HobbyClue.Common.Helpers
{
    /// <summary>
    /// Converts HTML
    /// </summary>
    public static class HtmlConverter
    {
        private static Regex paragraphMarks = new Regex("(<br/?>)|(</p>)", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private static Regex tags = new Regex("<[^>]*>", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Multiline);
        
        /// <summary>
        /// Converts HTML to plain text.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Plain text as string</returns>
        public static string ConvertToText(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return value;
            }
            else
            {
                value = paragraphMarks.Replace(value, Environment.NewLine);
                value = tags.Replace(value, String.Empty);
                
                return HttpUtility.HtmlDecode(value);
            }
        }
    }
}