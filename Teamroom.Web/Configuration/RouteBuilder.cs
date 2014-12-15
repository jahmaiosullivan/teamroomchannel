using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Forums.Web;

namespace HobbyClue.Web.Configuration
{
    public class RouteBuilder : IRouteBuilder
    {
        private readonly IUrlHelper urlHelper;

        public RouteBuilder(HttpContextBase httpContext)
            : this(new UrlHelperWrapper(new UrlHelper(new RequestContext(httpContext, new RouteData()), RouteTable.Routes)))
        {
        }

        public RouteBuilder(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public string GetRoute(string action, RouteValueDictionary values)
        {
            return urlHelper.Action(action, null, values, Uri.UriSchemeHttp, null);
        }

        public string GetRoute(string action, RouteValueDictionary values, string locale)
        {
            return urlHelper.Action(action, null, values, Uri.UriSchemeHttp, null);
        }

        public string GetApiRoute(string action, string controller)
        {
            return urlHelper.RouteUrl("BasicApiRoute", new { httproute = "", controller = controller, action = action });
        }

        public string GetRoute(string action, string controller, object routeValues)
        {
            return urlHelper.Action(action, controller, routeValues, Uri.UriSchemeHttp);
        }

        public string GetRoute(string action, string controller, RouteValueDictionary routeValues)
        {
            return urlHelper.Action(action, controller, routeValues, Uri.UriSchemeHttp, null);
        }

        public string GetRoute(string action, string controller)
        {
            return urlHelper.Action(action, controller, new RouteValueDictionary(), Uri.UriSchemeHttp, null);
        }


        public string GetRouteByRouteName(string routeName, object routeValues)
        {
            return urlHelper.RouteUrl(routeName, routeValues);
        }

        public string GetRouteByRouteName(string routeName, RouteValueDictionary routeValues)
        {
            return urlHelper.RouteUrl(routeName, routeValues);
        }
   

    }
}
