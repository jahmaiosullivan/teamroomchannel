using System.Globalization;
using System.Text.RegularExpressions;

namespace HobbyClue.Common.Helpers
{
    public class BodyTextSanitizer : IBodyTextSanitizer
    {
        public string Sanitize(string bodyText)
        {
            bodyText = CleanHtml(bodyText, System.Threading.Thread.CurrentThread.CurrentUICulture);
            bodyText = RemovePositionAttribute(bodyText);
            return bodyText;
        }

        public string Sanitize(string bodyText, CultureInfo locale)
        {
            bodyText = CleanHtml(bodyText, locale);
            bodyText = RemovePositionAttribute(bodyText);
            return bodyText;
        }

        public string CleanHtml(string html, CultureInfo locale)
        {
            return StringSanitizer.CleanHtml(html, locale);
        }

        public string StripHtml(string html, CultureInfo locale)
        {
            return StringSanitizer.StripHtml(html, locale);
        }

        public string RemovePositionAttribute(string bodyText)
        {
            var positionElementPattern = new Regex(@"(?<position>position:\s*\w+)", RegexOptions.Singleline | RegexOptions.Compiled);
            var matches = positionElementPattern.Matches(bodyText);
            for (var i = matches.Count; i > 0; i--)
            {
                var match = matches[i - 1];
                bodyText = bodyText.Remove(match.Index, match.Length);
            }
            return bodyText;
        }

        public string CleanText(string text, CultureInfo locale)
        {
            return StringSanitizer.CleanText(text, locale);
        }

        public string CleanText(string text)
        {
            return CleanText(text, new CultureInfo("en-US"));
        }
        
    }
}
