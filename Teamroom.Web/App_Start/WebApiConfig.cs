using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using HobbyClue.Web.App_Start;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Teamroom.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            ConfigureFormatters(config);
            ConfigureRoutes(config);
            config.Services.Replace(typeof(IHttpControllerTypeResolver), new WebApiControllerResolver());
            
            var suffix = typeof(DefaultHttpControllerSelector).GetField("ControllerSuffix", BindingFlags.Static | BindingFlags.Public);
            if (suffix != null) suffix.SetValue(null, "ApiController");
        }


        public static void ConfigureRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute("EmailNotExists", "api/users/EmailNotExists", new { controller = "Users", action = "EmailNotExists" });
            config.Routes.MapHttpRoute("SetUserTag", "api/tag/SetUserTag", new { controller = "Tag", action = "SetUserTag", tagId = 0, isActive = false });
            config.Routes.MapHttpRoute("SetCity", "api/users/setcity", new { controller = "Users", action = "SetCity", city =  "", region = "" });
            config.Routes.MapHttpRoute("BasicApiActionRoute", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("BasicApiRoute", "api/{controller}/{id}", new { id = RouteParameter.Optional });
        }

        static void ConfigureFormatters(HttpConfiguration config)
        {
            var serializerSetting = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            };
            config.Formatters.JsonFormatter.SerializerSettings = serializerSetting;

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}
