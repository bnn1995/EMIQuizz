using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Voltaire.Startup))]
namespace Voltaire
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
