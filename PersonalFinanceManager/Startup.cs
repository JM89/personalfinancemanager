using Microsoft.Owin;
using Owin;
using PersonalFinanceManager.App_Start;
using System.Web.Mvc;

[assembly: OwinStartupAttribute(typeof(PersonalFinanceManager.Startup))]
namespace PersonalFinanceManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigureNinject(app);

            ModelBinders.Binders.DefaultBinder = new CustomModelBinder();
        }
    }
}
