using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Teamroom.Web
{
    //Based on http://www.strathweb.com/2013/02/but-i-dont-want-to-call-web-api-controllers-controller/

    public class WebApiControllerResolver : DefaultHttpControllerTypeResolver
    {
        public WebApiControllerResolver() : base(IsHttpEndpoint)
        {
            
        }

        internal static bool IsHttpEndpoint(Type t)
        {
           if (t == null) throw new ArgumentNullException("t");

           return t.IsClass && t.IsVisible && !t.IsAbstract && typeof(ApiController).IsAssignableFrom(t) && typeof(IHttpController).IsAssignableFrom(t);
        }
    }
}