using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcMyLibrary.Startup))]
namespace MvcMyLibrary
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
