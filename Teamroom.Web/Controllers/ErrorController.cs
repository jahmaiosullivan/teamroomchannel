using System.Web.Mvc;

namespace HobbyClue.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            Response.StatusCode = 500;
            return View();
        }

        [HttpGet]
        public ViewResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
    }
}
