using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using HobbyClue.Tests.Helpers;

namespace HobbyClue.Tests
{
    /// <summary>
    /// Base type for controller tests
    /// </summary>
    public class ControllerFacts<TClassUnderTest> : HttpFacts<TClassUnderTest> where TClassUnderTest : Controller
    {
        protected ControllerFacts()
        {
            CreateController();
        }

        private void CreateController()
        {
           
            ClassUnderTest.ControllerContext = CreateControllerContext();
            ClassUnderTest.Url = new UrlHelper(ClassUnderTest.ControllerContext.RequestContext);
        }

        private ControllerContext CreateControllerContext()
        {
            return new ControllerContext(new RequestContext(Get<HttpContextBase>(), new RouteData()), ClassUnderTest);
        }
    }
}
