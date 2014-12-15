using System;
using System.Web.Routing;
using Microsoft.Forums.Web;

namespace HobbyClue.Web.Configuration
{
    public class UrlHelperWrapper : IUrlHelper
    {
        private readonly System.Web.Mvc.UrlHelper urlHelper;
        
        public UrlHelperWrapper(System.Web.Mvc.UrlHelper urlHelper)
        {
            this.urlHelper = urlHelper;
        }

        public string Action(string actionName, string controllerName, object routeValues)
        {
            return urlHelper.Action(actionName, controllerName, routeValues);
        }

        public string Action(string actionName, string controllerName, object routeValues, string protocol)
        {
            return urlHelper.Action(actionName, controllerName, routeValues, protocol);
        }
        
        public string Action(string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            return urlHelper.Action(actionName, controllerName, routeValues, protocol, hostName);
        }

        public string ActionWithoutCurrentRouteData(string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName)
        {
            var helper = new System.Web.Mvc.UrlHelper(new RequestContext(urlHelper.RequestContext.HttpContext, new RouteData()));
            return helper.Action(actionName, controllerName, routeValues, protocol, hostName);
        }

        public string RouteUrl(string routeName, object routeValues)
        {
            return urlHelper.RouteUrl(routeName, routeValues);
        }

        public string RouteUrl(string routeName, RouteValueDictionary routeValues)
        {
            return urlHelper.RouteUrl(routeName, routeValues);
        }

        public Uri GetCurrentUrl()
        {
            return urlHelper.RequestContext.HttpContext.Request.Url;
        }

    }
}
