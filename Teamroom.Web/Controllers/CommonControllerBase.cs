using System.Web.Mvc;
using System.Web.Routing;

namespace HobbyClue.Web.Controllers
{
    public class CommonControllerBase : Controller
    {
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            InitializeCity();
        }

        public void InitializeCity()
        {
            var city = RouteData.Values["city"];
            var region = RouteData.Values["region"];

            if (city != null) ViewBag.CityName = city.ToString();
            if(region != null) ViewBag.Region = region.ToString();
        }

        protected override void HandleUnknownAction(string actionName)
        {
            if (!ControllerContext.IsChildAction)
                RedirectToAction("NotFound", "Error");
        }
    }
}