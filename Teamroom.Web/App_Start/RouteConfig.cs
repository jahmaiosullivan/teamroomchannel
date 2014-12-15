using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Teamroom.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            const string guidConstraint = @"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$";
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("FileUpload", "media/{action}", new { controller = "Media", action = "Index" });

            routes.MapRoute("ForumByName", "forum/{name}", new { controller = "Product", action = "List" });
            
            routes.MapRoute("Default", "{controller}/{action}/{id}", new { controller = "Home", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("ErrorNotFound", "{*path}", new { controller = "Error", action = "NotFound" });
        }
    }


     
    public class NonNullOrEmptyConstraing : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
            RouteDirection routeDirection)
        {
            return values[parameterName] != null && !string.IsNullOrEmpty(values[parameterName].ToString().Trim()) && !string.IsNullOrWhiteSpace(values[parameterName].ToString().Trim());
        }
    }

}