using System;
using System.Web.Routing;

namespace Microsoft.Forums.Web
{
    public interface IUrlHelper
    {
        string Action(string actionName, string controllerName, object routeValues);
        string Action(string actionName, string controllerName, object routeValues, string protocol);
        string Action(string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName);
        string ActionWithoutCurrentRouteData(string actionName, string controllerName, RouteValueDictionary routeValues, string protocol, string hostName);
        Uri GetCurrentUrl();
        string RouteUrl(string routeName, object routeValues);
        string RouteUrl(string routeName, RouteValueDictionary routeValues);
    }
}
