using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AddingClaimsToRoles.Startup))]
namespace AddingClaimsToRoles
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
