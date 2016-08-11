using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BeerPal.Startup))]
namespace BeerPal
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
