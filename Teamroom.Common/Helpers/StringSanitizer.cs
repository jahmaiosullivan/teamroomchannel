using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;

namespace HobbyClue.Common.Helpers
{
    /// <summary>
    /// sanitizer utility
    /// </summary>
    public static class StringSanitizer
    {
        private const string MscomForums = "MSCOMForums";
        private static readonly Regex UnsafeXmlChars = new Regex("[\x1C\x1D\x1E\x1F\x1A\x1B\x0B\x0C\x0F\x01-\x19]", RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);

        public static string CleanHtml(string html)
        {
            return CleanHtml(html, new CultureInfo("en-US"));
        }


        /// <summary>
        /// strips unsafe html, optionally cleans text
        /// </summary>
        /// <param name="html">raw string</param>
        /// <param name="locale">locale for filtration</param>
        /// <returns>safe html string</returns>
        public static string CleanHtml(string html, CultureInfo locale)
        {
            string cleaned = html;

            cleaned = StringHelper.GetSafeHtml(cleaned);

            //Why does GetSafeHtmlFragment do that to br's?
            cleaned = cleaned.Replace("<br>", "<br/>");

            return cleaned;
        }

        /// <summary>
        /// Cleans profane text
        /// </summary>
        /// <param name="text">raw text</param>
        /// <param name="locale">locale for filtration</param>
        public static string CleanText(string text, CultureInfo locale)
        {
            return UnsafeXmlChars.Replace(text ?? string.Empty, "_");
        }

        /// <summary>
        /// strips all HTML elements from the string, optionally cleans text
        /// </summary>
        /// <param name="html">raw string</param>
        /// <param name="locale">locale for filtration</param>
        /// <returns>plaintext</returns>
        public static string StripHtml(string html, CultureInfo locale)
        {
            return HtmlConverter.ConvertToText(html);
        }
    }
}
