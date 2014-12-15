using System.Web.Routing;

namespace HobbyClue.Web.Configuration
{
    public interface IRouteBuilder
    {
        string GetRoute(string action, RouteValueDictionary values);
        string GetRoute(string action, RouteValueDictionary values, string locale);
        string GetApiRoute(string action, string controller);

        string GetRoute(string action, string controller);
        string GetRoute(string action, string controller, object routeValues);
        string GetRoute(string action, string controller, RouteValueDictionary routeValues);
        string GetRouteByRouteName(string routeName, object routeValues);
        string GetRouteByRouteName(string routeName, RouteValueDictionary routeValues);
    }
}