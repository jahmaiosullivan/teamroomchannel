using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HobbyClue.Web.Helpers
{
    public class MvcUrlHelper : UrlHelper
    {
        private readonly HttpRequestBase requestBase;
        public MvcUrlHelper(HttpRequestBase requestBase)
            : base(requestBase.RequestContext, RouteTable.Routes)
        {
            this.requestBase = requestBase;
        }

        public bool IsCurrentUrl(string path)
        {
            return string.Compare(path, requestBase.Url.AbsolutePath, StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}