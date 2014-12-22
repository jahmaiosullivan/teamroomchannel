using System;
using System.Web;
using Microsoft.AspNet.Identity;
using Teamroom.Business.Providers;

namespace Teamroom.Web.Providers
{
    public class UserProvider : IUserProvider
    {
        private readonly HttpContextBase httpContext;
        public UserProvider(HttpContextBase httpContext)
        {
            this.httpContext = httpContext;
        }

        Guid currentUserId;
        public Guid CurrentUserId
        {
            get
            {
                if (currentUserId != Guid.Empty) return currentUserId;
                if (httpContext == null)
                    currentUserId = Guid.Empty;
                else
                    Guid.TryParse(httpContext.User.Identity.GetUserId(), out currentUserId);
                return currentUserId;
            }
            set { currentUserId = value; }
        }

        public bool IsAuthenticated { 
            get {
                return httpContext.User != null && httpContext.User.Identity.IsAuthenticated; 
            } 
        }
    }
}