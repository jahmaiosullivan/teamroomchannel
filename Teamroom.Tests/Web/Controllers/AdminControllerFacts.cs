using System.Linq;
using System.Web.Mvc;
using HobbyClue.Web.Areas.Admin.Controllers;
using HobbyClue.Web.Controllers;
using HobbyClue.Web.Nav;
using HobbyClue.Web.ViewModels;
using Xunit;

namespace HobbyClue.Tests.Web.Controllers
{
    public class AdminControllerFacts
    {
        public class ManageEvents
        {
            private readonly TestableAdminController controller = TestableAdminController.Create();

        }

        public class NewEvent
        {
            
        }

        public class TestableAdminController : ControllerFacts<AdminController>
        {
            public static TestableAdminController Create()
            {
                var controller = new TestableAdminController();
                return controller;
            }
        }
    }
}
