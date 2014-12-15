using System.Web.Mvc;

namespace HobbyClue.Web.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View("UserProfile");
        }
    }
}