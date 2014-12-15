using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace HobbyClue.Web.ViewModels
{
    public static class HtmlHelperExtensions
    {
        public static string Image(this HtmlHelper helper,
            string url,
            string altText,
            object htmlAttributes)
        {
            var builder = new TagBuilder("img");
            builder.Attributes.Add("src", url);
            builder.Attributes.Add("alt", altText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            return builder.ToString(TagRenderMode.SelfClosing);
        }

        public static string TokenHeaderValue(this HtmlHelper helper)
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }

        public static string DefaultDateFormat(this HtmlHelper helper)
        {
            return "M d yyyy";
        }
    }
}