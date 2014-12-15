using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HobbyClue.Web.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequiresAuthorizationAttribute : ActionFilterAttribute
    {
        public RequiresAuthorizationAttribute()
        {
            Order = 1;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //var currentUser = WebSecurity.GetUserId(HttpContext.Current.User.Identity.Name);
            Roles.GetRolesForUser(HttpContext.Current.User.Identity.Name);

            //Roles.AddUserToRole(HttpContext.Current.User.Identity.Name, "Admin");

            //var authorize = (Func<bool>)Delegate.CreateDelegate(typeof(Func<bool>), filterContext.Controller, AuthorizationMethod);

            //if (!authorize())
            //{
            //    if (!UserProvider.IsAuthenticated)
            //    {
            //        throw new NotAuthorizedException("You are not authorized for this action");
            //    }
            //}
        }

    }
}
