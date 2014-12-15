using System.Web;
using System.Web.Routing;
using HobbyClue.Web;
using Moq;
using Teamroom.Web;
using Xunit;

namespace HobbyClue.Tests.Configuration
{
    public class RouteConfigFacts
    {
        private readonly TestableRouteConfig routeConfig = TestableRouteConfig.Create();

        [Fact(Skip = "")]
        public void DefaultRouteWorks()
        {
            var routeData = routeConfig.GetRouteData("~/home");

            Assert.NotNull(routeData);
            routeConfig.AssertEqualCaseInsensitive("home", routeData.Values["Controller"].ToString().ToLower());
            routeConfig.AssertEqualCaseInsensitive("index", routeData.Values["action"].ToString().ToLower());
        }


        [Fact]
        public void RoutesCreateActivity()
        {
            var routeData = routeConfig.GetRouteData("~/admin/activity/create");

            Assert.NotNull(routeData);
            routeConfig.AssertEqualCaseInsensitive("activity", routeData.Values["Controller"].ToString().ToLower());
            routeConfig.AssertEqualCaseInsensitive("create", routeData.Values["action"].ToString().ToLower());
        }

        [Fact]
        public void RoutesAdminCategories()
        {
            var routeData = routeConfig.GetRouteData("~/admin/categories");

            Assert.NotNull(routeData);
            routeConfig.AssertEqualCaseInsensitive("admin", routeData.Values["Controller"].ToString().ToLower());
            routeConfig.AssertEqualCaseInsensitive("categories", routeData.Values["action"].ToString().ToLower());
        }

        
        public class TestableRouteConfig : RouteConfig
        {
            public RouteCollection Routes;
            
            TestableRouteConfig(RouteCollection routes)
            {
                Routes = routes;
                RegisterRoutes(routes);
            }

            public static TestableRouteConfig Create()
            {
                return new TestableRouteConfig(new RouteCollection());
            }

            public RouteData GetRouteData(string url)
            {
                var httpContextMock = new Mock<HttpContextBase>();
                httpContextMock.Setup(c => c.Request.AppRelativeCurrentExecutionFilePath).Returns(url);
                return Routes.GetRouteData(httpContextMock.Object);
            }

            public void AssertEqualCaseInsensitive(string expected, string actual)
            {
                Assert.Equal(expected.ToLower(), actual.ToLower());
            }

        }
    }
}
