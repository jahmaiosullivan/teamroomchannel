using System.Reflection;
using System.Web.Mvc;

namespace Microsoft.Com.Forums.Web.Controllers
{
    public class HttpVersionHeaderFilterAttribute : ActionFilterAttribute
    {
        static readonly string Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Headers.Add("x-STOBuild", "ForumsWeb-" + Version);
            base.OnResultExecuted(filterContext);
        }
    }
}