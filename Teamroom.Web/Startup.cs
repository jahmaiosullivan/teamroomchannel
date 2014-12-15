using Microsoft.Owin;
using Owin;
using Teamroom.Web;

[assembly: OwinStartup(typeof(Startup))]
namespace Teamroom.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
