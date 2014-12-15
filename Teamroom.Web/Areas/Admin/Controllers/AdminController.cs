using System.Web.Mvc;
using HobbyClue.Web.Areas.Admin.ModelBuilders;

namespace HobbyClue.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly ISidebarModelBuilder sidebarModelBuilder;
        public AdminController(ISidebarModelBuilder sidebarModelBuilder)
        {
            this.sidebarModelBuilder = sidebarModelBuilder;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult Emails()
        {
            return View();
        }
        
        [HttpGet]
        [ChildActionOnly]
        public ActionResult Sidebar()
        {
            var model = sidebarModelBuilder.BuildAdmin();
            return PartialView("AdminSideBar", model);
        }
    }
}
