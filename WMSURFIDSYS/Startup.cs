using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WMSURFIDSYS.Startup))]
namespace WMSURFIDSYS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
