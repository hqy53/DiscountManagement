using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DiscountManagement.Startup))]
namespace DiscountManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
