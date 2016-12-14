using Microsoft.Owin;
using MyMovies.Web;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace MyMovies.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
