using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using HobbyClue.Web.AutoMappings;
using HobbyClue.Web.Configuration;
using Teamroom.Web.Configuration;

namespace Teamroom.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            SetupFilterProvider();
            var factory = new StructureMapControllerFactory(WebContainer.Current);
            ControllerBuilder.Current.SetControllerFactory(factory);
       

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            
            SetupAutomappings();
        }
        
        private static void SetupFilterProvider()
        {
            var oldProvider = FilterProviders.Providers.Single(f => f is FilterAttributeFilterProvider);
            FilterProviders.Providers.Remove(oldProvider);
            var provider = new StructureMapFilterProvider();
            FilterProviders.Providers.Add(provider);
        }

        public static void SetupAutomappings()
        {
            WebMapping.Configure(WebContainer.Current);
        }

    }
}