using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ServicesoAuthWebTester.Startup))]
namespace ServicesoAuthWebTester
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
