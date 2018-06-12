using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProvidersApp.Startup))]
namespace ProvidersApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
