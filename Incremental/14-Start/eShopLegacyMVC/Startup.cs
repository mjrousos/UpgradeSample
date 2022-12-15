using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(eShopLegacyMVC.Startup))]
namespace eShopLegacyMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
