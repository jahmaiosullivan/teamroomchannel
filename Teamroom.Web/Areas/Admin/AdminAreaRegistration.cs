using System.Web.Mvc;

namespace HobbyClue.Web.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            const string adminNamespace = "HobbyClue.Web.Areas.Admin.Controllers";


            context.MapRoute(
                "admin_default",
                "admin",
                new { controller="Admin", action = "Index" },
                new[] { adminNamespace }
            );

            context.MapRoute(
                "admin_action",
                "admin/{action}/{id}",
                new { controller="Admin", id = UrlParameter.Optional },
                new[] { adminNamespace }
            );

            context.MapRoute(
                "admin_all",
                "admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { adminNamespace }
            );
        }
    }
}