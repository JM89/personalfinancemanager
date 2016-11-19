using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PersonalFinanceManager.Startup))]
namespace PersonalFinanceManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureNinject(app);
        }
    }
}
