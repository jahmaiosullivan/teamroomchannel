using HobbyClue.Web.Configuration;
using Microsoft.Web.WebPages.OAuth;
using Teamroom.Common.Configuration;
using Teamroom.Web.Configuration;

namespace Teamroom.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            var configuration = WebContainer.Current.GetInstance<IConfiguration>();
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");


            OAuthWebSecurity.RegisterFacebookClient(
                configuration.FacebookAppId,
                configuration.FacebookAppSecret
                );

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
