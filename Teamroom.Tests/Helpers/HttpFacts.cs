using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.Web.Routing;
using Moq;

namespace HobbyClue.Tests.Helpers
{
    public class HttpFacts<TClassUnderTest> : Facts<TClassUnderTest> where TClassUnderTest : class
    {
        private static readonly object RouteLockObject = new object();

        public Mock<HttpContextBase> MockHttpContext
        {
            get
            {
                return Mock<HttpContextBase>();
            }
        }

        public Mock<HttpResponseBase> MockResponse
        {
            get
            {
                return Mock<HttpResponseBase>();
            }
        }

        public Mock<HttpRequestBase> MockRequest
        {
            get
            {
                return Mock<HttpRequestBase>();
            }
        }

        public HttpContextBase HttpContext
        {
            get
            {
                return Mock<HttpContextBase>().Object;
            }
        }

        public HttpResponseBase Response
        {
            get
            {
                return Mock<HttpResponseBase>().Object;
            }
        }

        public HttpRequestBase Request
        {
            get
            {
                return Mock<HttpRequestBase>().Object;
            }
        }

        public IDictionary ContextItems { get; private set; }

        static HttpFacts()
        {
        }

        public HttpFacts(Action<RouteCollection> routeRegistrar)
            : this()
        {
            lock (RouteLockObject)
            {
                if (RouteTable.Routes.Count > 0)
                    RouteTable.Routes.Clear();
                routeRegistrar(RouteTable.Routes);
            }
        }

        public HttpFacts()
        {
            SetupHttpProperties();
        }

        private void SetupHttpProperties()
        {
            ContextItems = new Hashtable();
            MockRequest.SetupGet(p => p.Files).Returns(Get<HttpFileCollectionBase>());
            MockRequest.SetupGet(p => p.QueryString).Returns(new NameValueCollection());
            MockRequest.SetupGet(p => p.Cookies).Returns(new HttpCookieCollection());
            MockRequest.SetupGet(p => p.Form).Returns(new NameValueCollection());
            MockRequest.SetupGet(p => p.Headers).Returns(new NameValueCollection());
            MockRequest.SetupProperty(p => p.ContentType);
            MockRequest.SetupProperty(p => p.ContentEncoding);
            MockRequest.SetupGet(p => p.Url).Returns(new Uri("http://tempuri.org"));
            MockRequest.SetupGet(r => r.Browser).Returns(Get<HttpBrowserCapabilitiesBase>());
            MockRequest.SetupGet(x => x.HttpMethod).Returns("GET");
            MockResponse.SetupAllProperties();
            MockResponse.SetupGet(r => r.Cache).Returns(Get<HttpCachePolicyBase>());
            MockResponse.SetupGet(r => r.Headers).Returns(new NameValueCollection());
            MockResponse.SetupGet(r => r.Cookies).Returns(new HttpCookieCollection());
            MockResponse.Setup(r => r.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            MockHttpContext.SetupGet(x => x.Request).Returns(Get<HttpRequestBase>());
            MockHttpContext.SetupGet(x => x.Response).Returns(Get<HttpResponseBase>());
            MockHttpContext.SetupGet(x => x.Items).Returns(ContextItems);
            MockHttpContext.SetupGet(x => x.Server).Returns(Get<HttpServerUtilityBase>());
            MockHttpContext.SetupProperty(x => x.User);
        }
    }
}
