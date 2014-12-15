using System.Web;
using System.Web.Mvc;
using GeoLib.Models;
using HobbyClue.Business.Providers;
using HobbyClue.Business.Services;
using HobbyClue.Data.Models;
using Teamroom.Web.Configuration;

namespace Teamroom.Web.Views
{
    public abstract class BaseWebViewPage<T> : WebViewPage<T>
    {
        public string CurrentUrl { get; set; }

        public bool IsAuthenticated { get; set; }

        public GeoPlace CurrentLocation { get; set; }

        public User CurrentUser { get; set; }
        
        public override void InitHelpers()
        {
            var userService = WebContainer.Current.GetInstance<IUserService>();
            var userProvider = WebContainer.Current.GetInstance<IUserProvider>();
            CurrentUser = userService.GetById(userProvider.CurrentUserId);
            //var placeService = WebContainer.Current.GetInstance<IGeoService>();
            //CurrentLocation = placeService.Get("107.22.231.148");
            CurrentLocation = new GeoPlace();
 
            CurrentUrl = HttpContext.Current.Request.Url.ToString();
            IsAuthenticated = HttpContext.Current != null && HttpContext.Current.User != null && HttpContext.Current.User.Identity != null && HttpContext.Current.User.Identity.IsAuthenticated;
            base.InitHelpers();
        }
    }

    public abstract class BaseWebViewPage : BaseWebViewPage<dynamic> { }
}